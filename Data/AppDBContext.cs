

using Microsoft.EntityFrameworkCore;
using System.Transactions;
using TransactionAPIApplication.Models;

namespace TransactionAPIApplication.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }    


        // ask matthew
        public DbSet <TransactionModel> Transactions { get; set; }
    }
}
