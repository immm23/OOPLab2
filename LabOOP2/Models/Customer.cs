namespace LabOOP2.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Address { get; set; }
        public virtual Passport? Passport { get; set; }
        public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public virtual BankAccount? BankAccount { get; set; }
    }
}
