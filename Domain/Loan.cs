namespace LabOOP2.Domain
{
    public class Loan
    {
        public Guid Id { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime TillDate { get; private set; }
        public DateTime FromDate { get; private set; }
        public int Percentage { get; private set; }
        public decimal FinalSum { get; private set; }
        public decimal PayedOffAmount { get; private set; }
        public bool IsPayedOff { get; private set; }

        private Loan() { }

        public Loan(Guid id,
            decimal amount,
            int duration,
            int percentage)
        {
            Id = id;
            Amount = amount;
            TillDate = DateTime.Now.AddMonths(duration);
            FromDate = DateTime.Now;
            Percentage = percentage;
            IsPayedOff = false;
            FinalSum = CalculateFinalSum(amount, percentage, duration);
        }

        public void PayOff(decimal amount)
        {
            if (IsPayedOff)
            {
                throw new InvalidOperationException("Loan is already payed off");
            }

            if(amount > FinalSum - PayedOffAmount || amount <= 0)
            {
                throw new InvalidOperationException($"{nameof(amount)} is invalid");
            }

            PayedOffAmount += amount;
            if(PayedOffAmount == FinalSum)
            {
                IsPayedOff = true;
            }
        }

        private decimal CalculateFinalSum(decimal amount, int percentage, int duration)
        {
            return amount * (decimal)Math.Pow(1 + percentage / 100, duration);
        }
    }
}
