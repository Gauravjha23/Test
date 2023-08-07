// Models/Transaction.cs
namespace API.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public DateTime TransactionDate { get; set; }
    }

    public enum TransactionType
    {
        Deposit,
        Withdrawal
    }
}