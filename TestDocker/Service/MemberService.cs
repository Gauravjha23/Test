// Services/MemberService.cs
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;

public class MemberService
{
    private readonly AppDbContext _context;

    public MemberService(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Member> GetMembers()
    {
        return _context.Members.ToList();
    }
    public Member AddMember(string name)
    {

        var member = new Member
        {
            Name = name,
            Balance = 0
        };

        _context.Members.Add(member);
        _context.SaveChanges();
        return member;
    }
    //public void AddDeposit(int  memberId, decimal depositAmount)
    //{
    //    var member = _context.Members.Find(memberId);

    //    if (member != null)
    //    {
    //        member.Balance += depositAmount;

    //        // Create a new deposit transaction record
    //        var deposit = new Transaction
    //        {
    //            MemberId = memberId,
    //            Amount = depositAmount,
    //            Type = TransactionType.Deposit,
    //            TransactionDate = DateTime.UtcNow
    //        };
    //        _context.Transactions.Add(deposit);

    //        _context.SaveChanges();
    //    }
    //}
    public void AddDeposit(int memberId, decimal depositAmount)
    {
        var member = _context.Members.Find(memberId);

        if (member != null)
        {
            member.Balance += depositAmount;

            // Create a new deposit transaction record
            var deposit = new Transaction
            {
                MemberId = memberId,
                Amount = depositAmount,
                Type = TransactionType.Deposit,
                TransactionDate = DateTime.UtcNow
            };
            _context.Transactions.Add(deposit);

            _context.SaveChanges();
        }
    }

    public void AddWithdrawal(int memberId, decimal withdrawal)
    {
        var member = _context.Members.Find(memberId);

        if (member != null)
        {
            member.Balance -= withdrawal;

            // Create a new deposit transaction record
            var transaction = new Transaction
            {
                MemberId = memberId,
                Amount = withdrawal,
                Type = TransactionType.Withdrawal,
                TransactionDate = DateTime.UtcNow
            };
            _context.Transactions.Add(transaction);

            _context.SaveChanges();
        }
    }

    public decimal GetDueAmount(int memberId)
    {
        var member = _context.Members.Find(memberId);
        new List<Deposit>();
        new List<Withdrawal>();
        if (member != null)
        {
            new List<Deposit>();
            // Calculate the total deposits and withdrawals for the member
            decimal totalDeposits = member.Deposits.Sum(d => d.Amount);
            decimal totalWithdrawals = member.Withdrawals.Sum(w => w.Amount);

            return totalDeposits - totalWithdrawals;
        }

        return 0;
    }
}

   
