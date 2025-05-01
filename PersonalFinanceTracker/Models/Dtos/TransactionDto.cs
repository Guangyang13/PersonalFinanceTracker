using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Models.Dtos
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public TransactionType Type { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime LastModified { get; set; }

    }
}
