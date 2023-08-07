using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Models
{
    public class Member
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string Name { get; set; }
        public decimal Balance { get; set; } = 0;
        [JsonIgnore]
        public ICollection<Deposit>? Deposits { get; set; } = null;
        [JsonIgnore]
        public ICollection<Withdrawal>? Withdrawals { get; set; } = null;
        [JsonIgnore]
        public ICollection<Transaction>? Transactions { get; set; } = new List<Transaction>();
    }
}

