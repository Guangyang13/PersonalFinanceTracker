using CommunityToolkit.Mvvm.ComponentModel;
using PersonalFinanceTracker.Mapper;
using PersonalFinanceTracker.Models.Dtos;
using PersonalFinanceTracker.Models.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.ViewModels
{
    public partial class TransactionVM : ObservableObject
    {
        private readonly Transaction _transaction;

        public Guid Id { get; set; }

        [ObservableProperty]
        private TransactionType _type;

        [ObservableProperty]
        private DateTime _date;

        [ObservableProperty]
        private decimal _amount;

        [ObservableProperty]
        private string _category = string.Empty;

        [ObservableProperty]
        private string _description = string.Empty;


        [ObservableProperty]
        private bool _IsSynced = false;

        [ObservableProperty]
        private bool _IsDeleted = false;

        [ObservableProperty]
        private DateTime _lastModified = DateTime.Now;

        public TransactionVM(Transaction? txn = null)
        {
            if (txn == null)
                _transaction = new();
            else
                _transaction = txn;

            Id = _transaction.Id;
            Type = _transaction.Type;
            Date = _transaction.Date;
            Amount = _transaction.Amount;
            Category = _transaction.Category;
            Description = _transaction.Description;

            IsSynced = _transaction.IsSynced;
            IsDeleted = _transaction.IsDeleted;
            LastModified = _transaction.LastModified;
        }

        public TransactionVM(TransactionDto? dto = null)
        {
            if (dto == null)
                _transaction = new();
            else
                _transaction = TransactionMapper.ToEntity(dto);

            Id = _transaction.Id;
            Type = _transaction.Type;
            Date = _transaction.Date;
            Amount = _transaction.Amount;
            Category = _transaction.Category;
            Description = _transaction.Description;

            IsSynced = _transaction.IsSynced;
            IsDeleted = _transaction.IsDeleted;
            LastModified = _transaction.LastModified;
        }


        public TransactionVM(TransactionType type, DateTime date, decimal amount, string category, string description)
        {
            _transaction = new Transaction(type, date, amount, category, description);

            Id = _transaction.Id;
            Type = _transaction.Type;
            Date = _transaction.Date;
            Amount = _transaction.Amount;
            Category = _transaction.Category;
            Description = _transaction.Description;

            IsSynced = _transaction.IsSynced;
            IsDeleted = _transaction.IsDeleted;
            LastModified = _transaction.LastModified;
        }

        public void Update(TransactionType type, DateTime date, decimal amount, string category, string description)
        {
            Type = type;
            Date = date;
            Amount = amount;
            Category = category;
            Description = description;
            LastModified = DateTime.Now;
        }

        public void Update(TransactionDto dto)
        {
            Type = dto.Type;
            Amount = dto.Amount;
            Date = dto.Date;
            Category = dto.Category;
            Description = dto.Description;
            LastModified = DateTime.Now;
        }


        public void SetSynced(bool isSynced)
        {
            IsSynced = isSynced;
            LastModified = DateTime.Now;
        }

        public void SetDeleted(bool isSynced)
        {
            IsSynced = isSynced;
            LastModified = DateTime.Now;
        }


        partial void OnTypeChanged(TransactionType oldValue, TransactionType newValue) => _transaction.Type = newValue;
        partial void OnAmountChanged(decimal oldValue, decimal newValue) => _transaction.Amount = newValue;
        partial void OnDateChanged(DateTime oldValue, DateTime newValue) => _transaction.Date = newValue;
        partial void OnCategoryChanged(string? oldValue, string newValue) => _transaction.Category = newValue;
        partial void OnDescriptionChanged(string? oldValue, string newValue) => _transaction.Description = newValue;

        public Transaction ToTransaction() => _transaction;
    }
}
