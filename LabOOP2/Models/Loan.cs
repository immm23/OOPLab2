namespace LabOOP2.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime TillDate { get; set; }
        public DateTime FromDate { get; set; }
        public decimal Percentage { get; set; }
        public virtual Customer? Customer { get; set; }  
    }
}
