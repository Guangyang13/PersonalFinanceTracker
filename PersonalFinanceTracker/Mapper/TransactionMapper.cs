using PersonalFinanceTracker.Models.Dtos;
using PersonalFinanceTracker.Models.Entity;
using PersonalFinanceTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Mapper
{
    public static class TransactionMapper
    {
        public static Transaction ToEntity(TransactionDto dto)
        {
            return new Transaction()
            {
                Id = dto.Id,
                Date = dto.Date,
                Type = dto.Type,
                Amount = dto.Amount,
                Category = dto.Category,
                Description = dto.Description,
                LastModified = dto.LastModified
            };
        }

        public static Transaction ToEntity(TransactionVM vm)
        {
            return new Transaction()
            {
                Id = vm.Id,
                Date = vm.Date,
                Type = vm.Type,
                Amount = vm.Amount,
                Category = vm.Category,
                Description = vm.Description,
                LastModified = vm.LastModified
            };
        }

        public static TransactionDto ToDto(Transaction entity)
        {
            return new TransactionDto()
            {
                Id = entity.Id,
                Date = entity.Date,
                Type = entity.Type,
                Amount = entity.Amount,
                Category = entity.Category,
                Description = entity.Description,
                LastModified = entity.LastModified
            };
        }

        public static TransactionDto ToDto(TransactionVM vm)
        {
            return new TransactionDto()
            {
                Id = vm.Id,
                Date = vm.Date,
                Type = vm.Type,
                Amount = vm.Amount,
                Category = vm.Category,
                Description = vm.Description,
                LastModified = vm.LastModified
            };
        }

        public static TransactionVM ToVM(Transaction entity)
        {
            return new TransactionVM(entity);
        }

        public static TransactionVM ToVM(TransactionDto dto)
        {
            return new TransactionVM(dto);
        }
        

    }
}
