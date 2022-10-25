using Amazon.DynamoDBv2.DataModel;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TransactionAPIApplication.DataContracts
{
    public class CreateTransactionRequest
    {
        [Required]
        public string Type { get; set; }

        [DisplayName("Amount")]
        [RegularExpression(@"[0-9]+(\.([0-9]{4,}|000|00|0))?", ErrorMessage ="Non negative number")] // regex for non-negative numbers and OPTIONAL decimal
        public int Amount { get; set; }

        // Gross Amount
    }

    public class TransactionResponse
    {
       
        public string Id { get; set; }

       
        public string Type { get; set; }

       
        public int Amount { get; set; }

        
    }

}
