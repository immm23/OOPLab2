namespace LabOOP2.Domain
{
    public class Transaction
    {
        public Guid Id { get; private set; }
        public decimal Amount { get; private set; }
        public AccountType FromAccount { get; private set; }
        public AccountType ToAccount { get; private set; }
        public DateTime Time { get; private set; }
        
        private Transaction() { }

        public Transaction(Guid id,
            decimal amount,
            AccountType fromAccount,
            AccountType toAccount,
            DateTime time)
        {
            Id = id;
            Amount = amount;
            FromAccount = fromAccount;
            ToAccount = toAccount;
            Time = time;
        }
    }
}
