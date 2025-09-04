using Microsoft.EntityFrameworkCore;
using MVCAssignment.Models;
using MVCAssignment.ViewModels;

namespace MVCAssignment.Models
{
    public class AppDbContext :DbContext
    {
        public DbSet<Product> ProductsTable { get; set; }
        public DbSet<Customer> CustomersTabel{ get; set; }
        public DbSet<Cart> CartTable{ get; set; }
        public DbSet<TransactionHistory> transactionHistories{ get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-6R4OII1\SQLEXPRESS;Database=MVCEcomProduct;Trusted_Connection=True;");
        }
        public DbSet<MVCAssignment.ViewModels.CartViewModel> CartViewModel { get; set; }
        //public DbSet<MVCAssignment.Models.Customer> Customer { get; set; }
    }
}
