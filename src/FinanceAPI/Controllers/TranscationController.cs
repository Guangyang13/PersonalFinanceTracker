using FinanceAPI.Data;
using FinanceAPI.Mapper;
using FinanceAPI.Models.Dtos;
using FinanceAPI.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FinanceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public TransactionController(ApiDbContext context)
        {
            _context = context;
        }

        private string GetUsername() => User.Identity?.Name!;

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var username = GetUsername();
            var transcations = await _context.Transactions
                .Where(txn => txn.Username == username)
                .Select(txn => TransactionMapper.ToDto(txn))
                .ToListAsync();

            return Ok(transcations);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] TransactionDto dto)
        {
            if (await _context.Transactions.AnyAsync(txn => txn.Id == dto.Id))
                return BadRequest("Transcation already exists");

            var username = GetUsername();
            _context.Transactions.Add(TransactionMapper.ToEntity(username, dto));
            await _context.SaveChangesAsync();
            return Ok(dto);
        }

        [HttpPost("batch")]
        public async Task<IActionResult> CreateBatchAsync([FromBody] List<TransactionDto> dtos)
        {
            var transactionIds = dtos.Select(d => d.Id).ToList();

            if (await _context.Transactions.AnyAsync(txn => transactionIds.Contains(txn.Id)))
                return BadRequest("One or more transcations already exists");


            var username = GetUsername();
            var transactionsToAdd = dtos.Select(txn => TransactionMapper.ToEntity(username, txn)).ToList();
            _context.Transactions.AddRange(transactionsToAdd);
            await _context.SaveChangesAsync();

            return Ok(new { transactionsToAdd.Count });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] TransactionDto dto)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null) 
                return NotFound();

            transaction.Update(dto);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("batch")]
        public async Task<IActionResult> UpdateBatchAsync([FromBody] List<TransactionDto> dtos)
        {
            var transactionIds = dtos.Select(d => d.Id).ToList();

            var existingRecords = await _context.Transactions
                .Where(txn => transactionIds.Contains(txn.Id))
                .ToListAsync();

            foreach (var dto in dtos)
            {
                var transaction = existingRecords.Find(t => t.Id == dto.Id);

                if (transaction == null)
                    return BadRequest("One or more transcations not exists");

                transaction.Update(dto);
            }

            await _context.SaveChangesAsync();
            return Ok(new { Updated = dtos.Count });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var username = GetUsername();
            var transcation = await _context.Transactions.FindAsync(id);

            if (transcation == null)
                return NotFound();

            if (transcation.Username != username)
                return Forbid();

            _context.Transactions.Remove(transcation);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
