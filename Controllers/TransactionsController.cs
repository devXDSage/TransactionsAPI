using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Threading.Tasks;
using TransactionAPIApplication.DataContracts;
using TransactionAPIApplication.Models;
using TransactionAPIApplication.Filters;
using Microsoft.AspNetCore.Authorization;

namespace TransactionAPIApplication.Controllers
{
    [Route("api/Transactions")]
   
    [ApiController]//   

   
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<TransactionsController> _logger;
        private readonly IMapper _mapper;   // dependecy injection + IOC
        
        public TransactionsController(ITransactionRepository transactionRepository, ILogger<TransactionsController> logger, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _logger = logger;
            _mapper = mapper;

        }

        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {

            //var product = await _dynamoDBContext.LoadAsync<Product>(category, productName);
            //return Ok(product);
            _logger.LogInformation("GET api/Transactions called");
            var transaction = new TransactionModel();
            try
            {
                
                // retrieved re
                return Ok();
            }

            catch (Exception ex) // how to test try catch blocks?
            {
                _logger.LogError(ex, "Exception while retreiving users");
                throw;
            }
        }

        //[HttpGet]
        [HttpGet ("{id:int}")]
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> Post([FromBody] CreateTransactionRequest createTransactionRequest) // tag: From body
        {
            _logger.LogInformation("POST api/Transactions called", createTransactionRequest);
            try
            {
                if(!ModelState.IsValid)
                {
                    _logger.LogError("ModelState validation failed");
                    return BadRequest(ModelState);
                }
                TransactionModel transaction = _mapper.Map<TransactionModel>(createTransactionRequest);              
                var tran = await _transactionRepository.Create(transaction);
                
                TransactionResponse response = _mapper.Map<TransactionResponse>(tran);

                return Ok(response);
            }

            catch (Exception ex) // how to test try catch blocks?
            {
                _logger.LogError(ex, "Exception while POSTing users");
                throw;
            }                   
        }

        [HttpPut("{id}")]
        [Authorize]
        public void Put([FromRoute] string id, [FromBody] TransactionModel tr)
        {
          
        }

        [HttpDelete("{id}")]
        [Authorize]
        /// use from route 
        // use repo class update method
        public void Delete([FromRoute] string id)
        {
           
        }

       
       
    }
 }
