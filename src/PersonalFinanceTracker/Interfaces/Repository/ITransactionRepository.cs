using PersonalFinanceTracker.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Interfaces.Repository
{
    public interface ITransactionRepository
    {
        List<Transaction> GetBatch();
        bool Create(Transaction transaction);
        bool Update(Transaction transaction);
        bool Delete(Guid id);
        bool SoftDelete(Guid id);
        bool SetSynced(Guid id, bool isSync);
    }
}
