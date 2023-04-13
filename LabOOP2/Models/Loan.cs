using System.Text.Json.Serialization;

namespace LabOOP2.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime TillDate { get; set; }
        public DateTime FromDate { get; set; }
        public decimal Percentage { get; set; }
        public decimal FinalSum { get; set; }
        public decimal PayedOff { get; set; }
        [JsonIgnore]
        public virtual Customer? Customer { get; set; }  
    }
}
