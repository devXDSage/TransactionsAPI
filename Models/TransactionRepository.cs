using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TransactionAPIApplication.Data;
using TransactionAPIApplication.Controllers;

// integrate logger again
namespace TransactionAPIApplication.Models
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<TransactionModel>> GetAll();
        Task <TransactionModel> Get(int id);
        Task<TransactionModel> Create(TransactionModel transaction);

        Task<TransactionModel> Update(int id, TransactionModel transaction);
        Task<TransactionModel> Delete(int id);
    }
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDBContext _appDBContext;
        private readonly ILogger<TransactionRepository> _logger;

        public TransactionRepository(AppDBContext appDBContext, ILogger<TransactionRepository> logger)
        {
            _appDBContext = appDBContext;
            _logger = logger;
        }

        
        public async Task<TransactionModel> Create(TransactionModel transaction)
        {
            _appDBContext.Transactions.Add(transaction);
            await _appDBContext.SaveChangesAsync();
            _logger.LogInformation("Record created");
            return transaction;
        }

        public async Task<TransactionModel> Update(int id, TransactionModel transaction)
        {
            TransactionModel? dbRecord = await _appDBContext.Transactions.FirstOrDefaultAsync(x => x.Id == id);

            if(dbRecord == null)
            {
                return null;
            }
            dbRecord.Amount = transaction.Amount;
            dbRecord.Type = transaction.Type;
            _appDBContext.Transactions.Update(dbRecord);
            await _appDBContext.SaveChangesAsync();
            _logger.LogInformation("Record updated");
            return dbRecord;
        }

        public async Task <TransactionModel> Delete(int id)
        {
            TransactionModel? dbRecord = await _appDBContext.Transactions.FirstOrDefaultAsync(x => x.Id == id);

            if (dbRecord == null)
            {
                return null;
            }

            _appDBContext?.Transactions.Remove(dbRecord);
            await _appDBContext.SaveChangesAsync();
            _logger.LogInformation("Record deleted");
            return dbRecord;
        }

        public async Task<TransactionModel> Get(int id)
        {
            TransactionModel? dbRecord = await _appDBContext.Transactions.FirstOrDefaultAsync(x => x.Id == id);
            return dbRecord;
        }

        public async Task<IEnumerable<TransactionModel>> GetAll()
        {
            _logger.LogInformation("All records requested");
            return await _appDBContext.Transactions.ToListAsync();
        }
    }
}
