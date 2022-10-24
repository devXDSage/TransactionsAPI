using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TransactionAPIApplication.Data;
using System;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;


// add ModelState 


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
        private readonly IMapper _mapper;

        public TransactionRepository(AppDBContext appDBContext, ILogger<TransactionRepository> logger, IDynamoDBContext dynamoDBContext, IMapper mapper)
        {
            _appDBContext = appDBContext;
            _logger = logger;
            _dynamoDBContext = dynamoDBContext;
            _mapper = mapper;
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
            TransactionModel? dbRecord = await _appDBContext.Transactions.FirstOrDefaultAsync(x => x.Id == id);  // LINQ

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
            //can use query -
            TransactionModel? dbRecord = await _dynamoDBContext.LoadAsync<TransactionModel>(id);

         //   TransactionModel? dbRecord = await _appDBContext.Transactions.FirstOrDefaultAsync(x => x.Id == id);
            return dbRecord;
        }

        
    }
}
