using CommunityToolkit.Mvvm.ComponentModel;
using LiteDB;
using Microsoft.VisualBasic;
using PersonalFinanceTracker.Migrations;
using PersonalFinanceTracker.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Models.Entity
{
    public class Transaction
    {
        [BsonId]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = string.Empty;
        public TransactionType Type { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;



        public bool IsSynced { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public DateTime LastModified { get; set; } = DateTime.Now;


        public Transaction(TransactionType type, DateTime date, decimal amount,  string category, string description)
        {
            Type = type;
            Amount = amount;
            Date = date;
            Category = category;
            Description = description;
        }

        public void Update(TransactionType type, DateTime date, decimal amount,  string category, string description)
        {
            Type = type;
            Amount = amount;
            Date = date;
            Category = category;
            Description = description;
            LastModified = DateTime.Now;
        }

        public void Update(Transaction txn)
        {
            Type = txn.Type;
            Amount = txn.Amount;
            Date = txn.Date;
            Category = txn.Category;
            Description = txn.Description;
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

        public void SetDeleted(bool isDeleted)
        {
            IsDeleted = isDeleted;
            LastModified = DateTime.Now;
        }

        public void SetSynced(bool isSynced)
        {
            IsSynced = isSynced;
            LastModified = DateTime.Now;
        }

        public Transaction() { }
    }
}
