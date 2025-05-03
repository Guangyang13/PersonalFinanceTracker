using PersonalFinanceTracker.Models.Dtos;
using PersonalFinanceTracker.Models.Entity;
using PersonalFinanceTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Interfaces.Transactions
{
    public interface ITransactionService
    {
        Task<List<TransactionDto>> GetBatchAsync();
        Task<bool> CreateAsync(TransactionDto transaction);
        Task<bool> CreateBatchAsync(List<TransactionDto> transaction);
        Task<bool> UpdateAsync(TransactionDto transaction);
        Task<bool> UpdateBatchAsync(List<TransactionDto> transaction);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> SyncToLocal(List<TransactionVM> localTransaction);
    }
}
