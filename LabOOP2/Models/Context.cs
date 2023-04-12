using Microsoft.EntityFrameworkCore;

namespace LabOOP2.Models
{
    public class Context : DbContext
    {
        public virtual DbSet<BankAccount> BankAccounts { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Loan> Loans { get; set; }
        public virtual DbSet<Passport> Passports { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        public Context(DbContextOptions<Context> options)
            :base(options)
        {
        }
    }
}
