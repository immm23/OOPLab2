namespace LabOOP2.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public virtual Customer? Customer { get; set; }
        public decimal Emount { get; set; }
        public string? FromDescription { get; set; }
        public string? ToDescription { get; set; }
    }
}
