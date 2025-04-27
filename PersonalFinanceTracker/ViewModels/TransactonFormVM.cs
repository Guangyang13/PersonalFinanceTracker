using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;
using static System.Runtime.InteropServices.JavaScript.JSType;
using PersonalFinanceTracker.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using PersonalFinanceTracker.Interfaces.Navigation;
using PersonalFinanceTracker.Interfaces.Transactions;
using PersonalFinanceTracker.Interfaces.Repository;
using PersonalFinanceTracker.Models.Entity;
using PersonalFinanceTracker.Models.Dtos;
using PersonalFinanceTracker.Mapper;
using PersonalFinanceTracker.Services.Transactions;
using PersonalFinanceTracker.Interfaces.Auth;
using System.Windows.Media;
using System.Printing;
using System.Runtime.Serialization;

namespace PersonalFinanceTracker.ViewModels
{
    public enum EditMode
    {
        None,
        Create,
        Edit
    }

    public partial class TransactionFormVM : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<TransactionVM> _transactions = new();

        [ObservableProperty]
        private TransactionVM _selectedTransaction = null!;

        [ObservableProperty]
        private string _username = string.Empty;

        [ObservableProperty]
        private TransactionType _type = TransactionType.Income;

        [ObservableProperty]
        private decimal _amount = 0.00m;

        [ObservableProperty]
        private DateTime _date = DateTime.Today;

        [ObservableProperty]
        private string _category = string.Empty;

        [ObservableProperty]
        private string _description = string.Empty;

        [ObservableProperty]
        private string _statusMessage = string.Empty;

        [ObservableProperty]
        private Visibility _statusMessageVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private EditMode _editMode = EditMode.None;

        [ObservableProperty]
        private bool _isEditing = false;

        [ObservableProperty]
        private bool _isUpdateEnabled = false;


        private readonly INavigationService _navigationSvc;
        private readonly ITransactionService _transactionSvc;
        private readonly ITransactionRepository _transactionRepo;
        private readonly IUserSessionService _userSvc;

        public TransactionFormVM(INavigationService navigationService,
            ITransactionService transactionService,
            ITransactionRepository transactionRepository,
            IUserSessionService userSessionService)
        {
            _navigationSvc = navigationService;
            _transactionSvc = transactionService;
            _transactionRepo = transactionRepository;
            _userSvc = userSessionService;

            Username = _userSvc.Username;
        }

        [RelayCommand]
        private async Task GetTransactionsAsync()
        {
            var localTransactions = _transactionRepo.GetBatch();

            DisplayTransactions(localTransactions.Select(TransactionMapper.ToVM));

            if (!await _transactionSvc.SyncToLocal(Transactions.ToList()))
                // Logging
                return;

            //DisplayTransactions(localTransactions.Select(TransactionMapper.ToVM));
        }

        [RelayCommand]
        private async Task DeleteTransactionAsync(TransactionVM txnVM)
        {

            _transactionRepo.SoftDelete(txnVM.Id);

            if (!await _transactionSvc.DeleteAsync(txnVM.Id))
            {
                // Logging
                return;
            }

            _transactionRepo.Delete(txnVM.Id);
            Transactions.Remove(txnVM);
        }

        private async Task<bool> CreateTransactionAsync()
        {
            var transaction = new TransactionVM(Type, Date, Amount, Category, Description);
            _transactionRepo.Create(transaction.ToTransaction());

            Transactions.Add(transaction);

            var isSuccess = await _transactionSvc.CreateAsync(TransactionMapper.ToDto(transaction));
            if (isSuccess != transaction.IsSynced)
            {
                transaction.SetSynced(isSuccess);
                _transactionRepo.SetSynced(SelectedTransaction.Id, isSuccess);
            }

            return true;
        }

        private async Task<bool> UpdateTransactionAsync()
        {
            SelectedTransaction.Update(Type, Date, Amount, Category, Description);

            _transactionRepo.Update(SelectedTransaction.ToTransaction());

            var isSuccess = await _transactionSvc.UpdateAsync(TransactionMapper.ToDto(SelectedTransaction));
            if (isSuccess != SelectedTransaction.IsSynced)
            {
                SelectedTransaction.SetSynced(isSuccess);
                _transactionRepo.SetSynced(SelectedTransaction.Id, isSuccess);
            }

            return true;

        }


        [RelayCommand]
        private void Create()
        {
            Type = TransactionType.Income;
            Amount = 0.00m;
            Date = DateTime.Today;
            Category = string.Empty;
            Description = string.Empty;

            EditMode = EditMode.Create;
            IsEditing = true;
        }

        [RelayCommand]
        private void Update()
        {
            Type = SelectedTransaction.Type;
            Amount = SelectedTransaction.Amount;
            Date = SelectedTransaction.Date;
            Category = SelectedTransaction.Category;
            Description = SelectedTransaction.Description;

            EditMode = EditMode.Edit;
            IsEditing = true;
        }

        [RelayCommand]
        private void Cancel()
        {
            EditMode = EditMode.None;
            IsEditing = false;
        }

        [RelayCommand]
        private void Logout()
        {
            _userSvc.ClearSession();
            _navigationSvc.NavigateTo<LoginVM>();
        }

        [RelayCommand]
        private async Task ApplyAsync()
        {
            if (EditMode == EditMode.Create && !await CreateTransactionAsync())
            {
                StatusMessage = "Transaction failed to create. Please try again.";
                StatusMessageVisibility = Visibility.Visible;
                return;
            }
            else if (EditMode == EditMode.Edit && !await UpdateTransactionAsync())
            {
                StatusMessage = "Transaction failed to update. Please try again.";
                StatusMessageVisibility = Visibility.Visible;
                return;

            }

            IsEditing = false;
        }


        private void DisplayTransactions(IEnumerable<TransactionVM> transactions)
        {
            Transactions.Clear();
            foreach (var transaction in transactions)
            {
                Transactions.Add(transaction);
            }
        }

        partial void OnSelectedTransactionChanged(TransactionVM? oldValue, TransactionVM newValue)
        {
            IsUpdateEnabled = newValue != null && !IsEditing;
        }

        partial void OnIsEditingChanged(bool oldValue, bool newValue)
        {
            IsUpdateEnabled = !newValue && SelectedTransaction != null;
        }


    }
}
