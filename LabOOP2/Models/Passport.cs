namespace LabOOP2.Models
{
    public class Passport
    {
        public int Id { get; set; }
        public string? Seria { get; set; }
        public string? Number { get; set; }
        public string? IssuedByAuthority { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime IssuedDate { get; set; }
    }
}
