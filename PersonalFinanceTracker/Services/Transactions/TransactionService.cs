using PersonalFinanceTracker.Interfaces.Auth;
using PersonalFinanceTracker.Interfaces.Infrastructure;
using PersonalFinanceTracker.Interfaces.Repository;
using PersonalFinanceTracker.Interfaces.Transactions;
using PersonalFinanceTracker.Mapper;
using PersonalFinanceTracker.Models.Dtos;
using PersonalFinanceTracker.Models.Entity;
using PersonalFinanceTracker.Repositories;
using PersonalFinanceTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;

namespace PersonalFinanceTracker.Services.Transactions
{
    public class TransactionService : ITransactionService
    {
        private readonly HttpClient _client;
        private readonly INetworkService _networkSvc;
        private readonly IUserSessionService _userSvc;
        private readonly ITransactionRepository _transactionRepo;

        public TransactionService(
            INetworkService networkService,
            IUserSessionService userSessionService,
            ITransactionRepository transactionRepository,
            HttpClient client)
        {
            _userSvc = userSessionService;
            _networkSvc = networkService;
            _transactionRepo = transactionRepository;

            _client = client;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSvc.JwtToken);

        }

        public async Task<List<TransactionDto>> GetBatchAsync()
        {
            if (!await _networkSvc.IsServerReachableAsync())
            {
                // Logging
                return new List<TransactionDto>();
            }

            var response = await _client.GetAsync("api/transaction");
            return await response.Content.ReadFromJsonAsync<List<TransactionDto>>() ?? new List<TransactionDto>();
        }

        public async Task<bool> CreateAsync(TransactionDto dto)
        {
            if (!await _networkSvc.IsServerReachableAsync())
                // Logging
                return false;

            var response = await _client.PostAsJsonAsync("api/transaction", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CreateBatchAsync(List<TransactionDto> dtos)
        {
            if (!await _networkSvc.IsServerReachableAsync())
                // Logging
                return false;

            var response = await _client.PostAsJsonAsync("api/transaction/batch", dtos);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(TransactionDto dto)
        {
            if (!await _networkSvc.IsServerReachableAsync())
                // Logging
                return false;

            var response = await _client.PutAsJsonAsync($"api/transaction/{dto.Id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateBatchAsync(List<TransactionDto> dtos)
        {
            if (!await _networkSvc.IsServerReachableAsync())
                // Logging
                return false;

            var response = await _client.PutAsJsonAsync("api/transaction/batch", dtos);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (!await _networkSvc.IsServerReachableAsync())
                // Logging
                return false;

            var response = await _client.DeleteAsync($"api/transaction/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> SyncToLocal(List<TransactionVM> localTxnVMs)
        {

            var serverTransactions= await GetBatchAsync();
            var serverDict = serverTransactions.ToDictionary(t => t.Id);

            foreach (var localTxnVM in localTxnVMs)
            {
                if (!serverDict.TryGetValue(localTxnVM.Id, out var serverTxnDto))
                {
                    if (!await CreateAsync(TransactionMapper.ToDto(localTxnVM)))
                    {
                        // Logging
                        _transactionRepo.SetSynced(localTxnVM.Id, false);
                        return false;
                    }
                }

                else if (localTxnVM.IsDeleted == true)
                {
                    if (!await DeleteAsync(localTxnVM.Id))
                    {
                        // Logging
                        return false;
                    }

                    _transactionRepo.Delete(localTxnVM.Id);
                }

                else if (serverTxnDto.LastModified < localTxnVM.LastModified)
                {
                    if (!await UpdateAsync(TransactionMapper.ToDto(localTxnVM)))
                    {
                        // Logging
                        _transactionRepo.SetSynced(localTxnVM.Id, false);
                        return false;
                    }

                }

                else if (serverTxnDto.LastModified > localTxnVM.LastModified)
                {
                    localTxnVM.Update(serverTxnDto);
                    if (!_transactionRepo.Update(localTxnVM.ToTransaction()))
                    {
                        // Logging
                        _transactionRepo.SetSynced(localTxnVM.Id, false);
                        return false;
                    }

                }

                localTxnVM.SetSynced(true);
                _transactionRepo.SetSynced(localTxnVM.Id, true);
            }

            foreach (var serverTxnDto in serverDict.Values)
            {
                if (localTxnVMs.Any(txn => txn.Id == serverTxnDto.Id))
                    continue;

                var newTxnVM = new TransactionVM(serverTxnDto);
                localTxnVMs.Add(newTxnVM);
                if (!_transactionRepo.Create(newTxnVM.ToTransaction()))
                {
                    // Logging
                    return false;
                }

                newTxnVM.SetSynced(true);
                _transactionRepo.SetSynced(newTxnVM.Id, true);

            }

            return true;

        }


    }
}
