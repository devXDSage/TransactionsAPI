﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Transactions;
using TransactionAPIApplication.Models;

namespace TransactionAPIApplication.Controllers
{
    [Route("api/Transactions")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<HomeController> _logger; // dependecy injection + IOC


        public HomeController(ITransactionRepository transactionRepository, ILogger<HomeController> logger)
        {
            _transactionRepository = transactionRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("GET api/Transactions called");
            var transaction = new TransactionModel();
            var re = await _transactionRepository.GetAll();
            _logger.LogInformation("Users retreived");
           // retrieved re
            return Ok(re);
        }

        [HttpGet ("{id:int}")]
        public async Task<IActionResult> GetID([FromRoute] int id)
        {
            _logger.LogInformation("GET api/Transactions called with ID {id}", id);
            var transaction = new TransactionModel();
            var re = await _transactionRepository.Get(id);
            if (re == null)
            {
                _logger.LogInformation("No user found with the id: {id}" , id);
                return NotFound();
            }
            
            _logger.LogInformation("Users retreived");
            // retrieved re
            return Ok(re);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TransactionModel tr) // tag: From body
        {
            _logger.LogInformation("POST api/Transactions called", tr);
            await _transactionRepository.Create(tr);
            return Ok(new
            {
                Message = "Created"
            });;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] TransactionModel tr)
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
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            _logger.LogInformation("DELETE api/Transactions called with id: {id} ", id);
            await _transactionRepository.Delete(id);
            return Ok(new
            {
                Message = "Deleted"
            }) ;

        }

       
       
    }
 }