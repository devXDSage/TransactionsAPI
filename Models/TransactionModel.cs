using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.X86;

namespace TransactionAPIApplication.Models
{
    public class TransactionModel
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Type { get; set; } = string.Empty;

        [Required]
        public int Amount { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now; 


    }
}
