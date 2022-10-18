﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TransactionAPIApplication.Models;

namespace TransactionAPIApplication.Controllers
{
    [Route("api/Transactions")]
    [ApiController]
    public class TransactionsController : Controller
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<TransactionsController> _logger; // dependecy injection + IOC
        


        public TransactionsController(ITransactionRepository transactionRepository, ILogger<TransactionsController> logger)
        {
            _transactionRepository = transactionRepository;
            _logger = logger;
            

        }
         
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            //var product = await _dynamoDBContext.LoadAsync<Product>(category, productName);
            //return Ok(product);
            _logger.LogInformation("GET api/Transactions called");
            var transaction = new TransactionModel();
            try
            {
                var re = await _transactionRepository.GetAll();
                _logger.LogInformation("Users retreived");
                // retrieved re
                return Ok(re);
            }

            catch (Exception ex) // how to test try catch blocks?
            {
                _logger.LogError(ex, "Exception while retreiving users");
                throw;
            }
        }

        //[HttpGet]
        [HttpGet ("{id:int}")]
        public async Task<IActionResult> GetID([FromRoute] string id)
        {
            
            var transaction = new TransactionModel();
            var re = await _transactionRepository.Get(id);
            if (re == null)
            {
                _logger.LogInformation("No user found with the id: {id}" , id);
                return NotFound();
            }
            _logger.LogInformation("GET api/Transactions called with ID {id}", id);
            return Ok(re);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromRoute] string id,[FromBody] TransactionModel tr) // tag: From body
        {
            _logger.LogInformation("POST api/Transactions called", tr);
            try
            {
                await _transactionRepository.Create(tr);
                return Ok(new
                {
                    Message = "Created"
                }); ;
            }

            catch (Exception ex) // how to test try catch blocks?
            {
                _logger.LogError(ex, "Exception while POSTing users");
                throw;
            }

           
           
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] string id, [FromBody] TransactionModel tr)
        {
            _logger.LogInformation("PUT api/Transactions called. {id} =  , {data} =  ", id, tr );
            var result = await _transactionRepository.Update(id, tr);
            if(result == null)
            {
                _logger.LogInformation("Users could not be updated with the id: {id}", id);
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        /// use from route 
        // use repo class update method
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            _logger.LogInformation("DELETE api/Transactions called with id: {id} ", id);

            try
            {
                await _transactionRepository.Delete(id);
                return Ok(new
                {
                    Message = "Deleted"
                }); 
            }

            catch (Exception ex) // how to test try catch blocks?
            {
                _logger.LogError(ex, "Exception while DELETing users");
                throw;
            }

        }

       
       
    }
 }
