using FinanceAPI.Models.Dtos;

namespace FinanceAPI.Models.Entity
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public DateTime LastModified { get; set; }

        public void Update(TransactionDto dto)
        {
            Type = dto.Type;
            Amount = dto.Amount;
            Date = dto.Date;
            Category = dto.Category;
            Description = dto.Description;

            LastModified = dto.LastModified;
        }


    }
}
