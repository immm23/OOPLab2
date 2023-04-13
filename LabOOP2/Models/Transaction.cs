using System.Text.Json.Serialization;

namespace LabOOP2.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        [JsonIgnore]
        public virtual Customer? Customer { get; set; }
        public decimal Amount { get; set; }
        public string? FromDescription { get; set; }
        public string? ToDescription { get; set; }
        public DateTime Time { get; set; }
    }
}
