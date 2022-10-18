using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TransactionAPIApplication.Data;
using System;
using Amazon.DynamoDBv2.DataModel;

// integrate logger again

//update.create tranaction model
//make a request class to abstract transactions
//create transacion request
//create transation resposne model which will be needed to respond the model
//class for passing around (can have a third layer which copies the values)

namespace TransactionAPIApplication.Models
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<TransactionModel>> GetAll();
        Task <TransactionModel> Get(string id);
        Task<TransactionModel> Create(TransactionModel transaction);

        Task<TransactionModel> Update(string id, TransactionModel transaction);
        Task<TransactionModel> Delete(string id);
    }
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDBContext _appDBContext;
        private readonly IDynamoDBContext _dynamoDBContext;
        private readonly ILogger<TransactionRepository> _logger;

        public TransactionRepository(AppDBContext appDBContext, ILogger<TransactionRepository> logger, IDynamoDBContext dynamoDBContext)
        {
            _appDBContext = appDBContext;
            _logger = logger;
            _dynamoDBContext = dynamoDBContext;
        }
    
        public async Task<TransactionModel> Create(TransactionModel transaction)
        {

            Guid g = Guid.NewGuid();
            transaction.Id = g.ToString();
            //_appDBContext.Transactions.Add(transaction);
            await _dynamoDBContext.SaveAsync(transaction);
            //await _appDBContext.SaveChangesAsync();
            //await _dynamoDBContext.SaveAsync<>();
            _logger.LogInformation("Record created");
            return transaction;
        }

        public async Task<TransactionModel> Update(string id, TransactionModel transaction)
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

        public async Task <TransactionModel> Delete(string id)
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

        public async Task<TransactionModel> Get(string id)
        {

            TransactionModel? dbRecord = await _dynamoDBContext.LoadAsync<TransactionModel>(id);

         //   TransactionModel? dbRecord = await _appDBContext.Transactions.FirstOrDefaultAsync(x => x.Id == id);
            return dbRecord;
        }

        public async Task<IEnumerable<TransactionModel>> GetAll()
        {
           // _logger.LogInformation("All records requested");
            
           // var scanConditions = new List<ScanCondition>() { new ScanCondition("Id", ScanOperator.IsNotNull) };


           //var result = await _dynamoDBContext.ScanAsync<Reader>(scanConditions);

            throw new NotImplementedException();

        }
    }
}
