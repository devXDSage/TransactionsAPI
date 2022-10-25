using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;

namespace TransactionAPIApplication.Models
{
    public interface ITransactionRepository
    {    
        Task <TransactionModel> Get(string id);
        Task<TransactionModel> Create(TransactionModel transaction);
       void Update(string id, TransactionModel transaction);
       void Delete(string id);
    }
    public class TransactionRepository : ITransactionRepository
    {
        
        private readonly IDynamoDBContext _dynamoDBContext;
        private readonly ILogger<TransactionRepository> _logger;
        private readonly IMapper _mapper;

        public TransactionRepository(ILogger<TransactionRepository> logger, IDynamoDBContext dynamoDBContext, IMapper mapper)
        {          
            _logger = logger;
            _dynamoDBContext = dynamoDBContext;
            _mapper = mapper;
        }
    
        public async Task<TransactionModel> Create(TransactionModel transaction)
        {
            Guid g = Guid.NewGuid();
            transaction.Id = g.ToString();
            await _dynamoDBContext.SaveAsync(transaction);
            _logger.LogInformation("Record created");
            return transaction;
        }

        public void  Update(string id, TransactionModel transaction)
        {    
            _logger.LogInformation("Record updated");         
        }
        public void  Delete(string id)
        {
          
        }
        public async Task<TransactionModel> Get(string id)
        {
            TransactionModel? dbRecord = await _dynamoDBContext.LoadAsync<TransactionModel>(id);
            return dbRecord;
        }      
    }
}
