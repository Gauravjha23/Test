namespace API.Models
{
    public class Deposit
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }

        public int MemberId { get; set; }
        //public Member Member { get; set; }
    }
}
