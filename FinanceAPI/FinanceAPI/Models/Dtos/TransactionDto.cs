using FinanceAPI.Models.Entity;

namespace FinanceAPI.Models.Dtos
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public bool IsDeleted { get; set; }
        public DateTime LastModified { get; set; }

    }
}
