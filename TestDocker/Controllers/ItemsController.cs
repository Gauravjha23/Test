// Controllers/ItemsController.cs
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

//[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ItemsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly AuthService _authService;
    private readonly MemberService _memberService;

    public ItemsController(AppDbContext context, AuthService authService, MemberService memberService)
    {
        _context = context;
        _authService = authService;
        _memberService = memberService;
    }

    // GET: api/items
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
    {
        var members = _memberService.GetMembers();
        return Ok(members);
    }

    // Only Admin role can access these endpoints
    // POST: api/items
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Member>> CreateMember(Member member)
    {
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMembers), new { id = member.Id }, member);
    }

    // PUT: api/items/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateMember(int id, Member member)
    {
        if (id != member.Id)
        {
            return BadRequest();
        }

        _context.Entry(member).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/items/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteMember(int id)
    {
        var member = await _context.Members.FindAsync(id);

        if (member == null)
        {
            return NotFound();
        }

        _context.Members.Remove(member);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Custom endpoint to handle deposits for members
    // POST: api/items/deposit
    [HttpPost("deposit")]
    //[Authorize(Roles = "Admin")]
    public IActionResult AddDeposit(DepositModel depositModel)
    {
        _memberService.AddDeposit(depositModel.MemberId, depositModel.Amount);
        return Ok(new {depositModel.Amount, Message = "Deposit added successfully" });
    }

    // Custom endpoint to handle withdrawals for members
    // POST: api/items/withdrawal
    [HttpPost("withdrawal")]
    //[Authorize(Roles = "Admin")]
    public IActionResult AddWithdrawal(WithdrawalModel withdrawalModel)
    {
        _memberService.AddWithdrawal(withdrawalModel.MemberId, withdrawalModel.Amount);
        return Ok(new { Message = "Withdrawal added successfully" });
    }

    // Custom endpoint to get the due amount for a member
    // GET: api/items/due/{id}
    [HttpGet("due/{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetDueAmount(int id)
    {
        var dueAmount = _memberService.GetDueAmount(id);
        return Ok(new { DueAmount = dueAmount });
    }
}
