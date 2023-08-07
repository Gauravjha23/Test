// Controllers/AccountController.cs
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly UserManager<User> _userManager;
    private readonly MemberService _memberService;

    public AccountController(AuthService authService, UserManager<User> userManager,MemberService memberService)
    {
        _authService = authService;
        _userManager = userManager;
        _memberService=memberService;
    }

    [HttpPost("register/user")]
    public async Task<IActionResult> RegisterUser(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _authService.RegisterUserAsync(model.Username, model.Password);

            if (result)
            {
                return Ok(new { Message = "User registration successful" });
            }

            return BadRequest("User registration failed");
        }

        return BadRequest(ModelState);
    }

    [HttpPost("register/admin")]
    public async Task<IActionResult> RegisterAdmin(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _authService.RegisterAdminAsync(model.Username, model.Password);

            if (result)
            {
                return Ok(new { Message = "Admin registration successful" });
            }

            return BadRequest("Admin registration failed");
        }

        return BadRequest(ModelState);
    }
    [HttpPost("addmember")]
    [Authorize(Roles = "Admin")] // Only Admins can add new members
    public IActionResult AddMember(Member model)
    {
        if (ModelState.IsValid)
        {
            _memberService.AddMember(model.Name);

            return Ok(new { Message = "Member added successfully" });
        }

        return BadRequest(ModelState);
    }
    
    private string GetUserRole(string username)
    {
        var User = new User();
        //var user = _userManager.Users.fin..FirstOrDefault(u => u.Username == username);

        // Check if the username exists in the static dictionary and return the corresponding role
        if (User.Role =="Admin")
        {
            return User.Role="Admin";
        }

        // Default to "User" role if username not found in dictionary (you can adjust this as needed)
        return "User";
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {

        if (ModelState.IsValid)
         {

            if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                // Return a bad request response if the model or its properties are null/empty.
                return BadRequest("Username and password are required.");
            }
            var token = await _authService.LoginAsync(model.Username, model.Password);

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null)
            {
                var role = user.Role;
                if (token != null)
                {
                    return Ok(new { Token = token, Role = role });
                }

            }
            return BadRequest("Invalid login credentials");
        }

        return BadRequest(ModelState);
    }
}
