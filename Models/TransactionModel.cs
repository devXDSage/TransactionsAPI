using Amazon.DynamoDBv2.DataModel;
using System;

namespace TransactionAPIApplication.Models
{
    [DynamoDBTable("Orders")]
    public class TransactionModel
    {
        [DynamoDBHashKey("GID")]
        public string Id { get; set; }

        [DynamoDBProperty("Type")]
        public string Type { get; set; } = string.Empty;

        [DynamoDBProperty("Amount")]
        public int Amount { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now; 


    }
}
