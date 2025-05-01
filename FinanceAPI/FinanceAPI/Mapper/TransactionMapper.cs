using FinanceAPI.Models.Dtos;
using FinanceAPI.Models.Entity;

namespace FinanceAPI.Mapper
{
    public static class TransactionMapper
    {
        public static TransactionDto ToDto(Transaction transaction)
        {
            return new TransactionDto()
            {
                Id = transaction.Id,
                Date = transaction.Date,
                Type = transaction.Type,
                Amount = transaction.Amount,
                Category = transaction.Category,
                Description = transaction.Description,

                LastModified = transaction.LastModified
            };
        }

        public static Transaction ToEntity(string username, TransactionDto dto)
        {
            return new Transaction()
            {
                Id = dto.Id,
                Username = username,
                Date = dto.Date,
                Type = dto.Type,
                Amount = dto.Amount,
                Category = dto.Category,
                Description = dto.Description,

                LastModified = dto.LastModified
            };
        }


    }
}
