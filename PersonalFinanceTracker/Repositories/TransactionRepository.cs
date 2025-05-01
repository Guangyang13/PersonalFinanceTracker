using LiteDB;
using PersonalFinanceTracker.Interfaces.Auth;
using PersonalFinanceTracker.Interfaces.Repository;
using PersonalFinanceTracker.Mapper;
using PersonalFinanceTracker.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private string _dbPath = "finances_data.db";
        private const string _collectionName = "transactions";

        private readonly IUserSessionService _userSvc;

        public TransactionRepository(IUserSessionService userSessionService)
        {
            _userSvc = userSessionService;
        }

        public List<Transaction> GetBatch()
        {
            using var db = new LiteDatabase(_dbPath);
            return db.GetCollection<Transaction>(_collectionName).Find(t => t.Username == _userSvc.Username).ToList();
        }

        public bool Create(Transaction txn)
        {
            txn.Username = _userSvc.Username;

            using var db = new LiteDatabase(_dbPath);
            var column = db.GetCollection<Transaction>(_collectionName);
            if (column.FindById(txn.Id) != null)
                return false;

            column.Insert(txn);
            return true;
        }

        public bool Update(Transaction txn)
        {
            using var db = new LiteDatabase(_dbPath);
            var column = db.GetCollection<Transaction>(_collectionName);
            var transaction = column.FindById(txn.Id);
            if (transaction == null)
                return false;

            transaction.Update(txn);
            column.Update(transaction);
            return true;
        }

        public bool Delete(Guid id)
        {
            using var db = new LiteDatabase(_dbPath);
            var column = db.GetCollection<Transaction>(_collectionName);
            var transaction = column.FindById(id);
            if (transaction == null)
                return false;

            column.Delete(id);
            return true;
        }

        public bool SoftDelete(Guid id)
        {
            using var db = new LiteDatabase(_dbPath);
            var column = db.GetCollection<Transaction>(_collectionName);
            var transaction = column.FindById(id);
            if (transaction == null)
                return false;

            transaction.SetDeleted(true);
            column.Update(transaction);
            return true;
        }

        public bool SetSynced(Guid id, bool isSynced)
        {
            using var db = new LiteDatabase(_dbPath);
            var column = db.GetCollection<Transaction>(_collectionName);
            var txn = column.FindById(id);
            if (txn == null)
                return false;

            txn.SetSynced(isSynced);
            column.Update(txn);
            return true;

        }

    }
}
