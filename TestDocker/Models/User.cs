using Microsoft.AspNetCore.Identity;

namespace API.Models
{
    public class User: IdentityUser
    {
        public string Role { get; set; }
    }
}
