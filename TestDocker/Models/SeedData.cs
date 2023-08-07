using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using API.Models; // Make sure to include the correct namespace for your models.

public  class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var services = scope.ServiceProvider;
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<User>>();

            // Create the Admin role if it doesn't exist
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                var role = new IdentityRole { Name = "Admin" };
                _ = roleManager.CreateAsync(role).Result;
            }

            // Create the Admin user if it doesn't exist
            if (userManager.FindByNameAsync("admin").Result == null)
            {
                var user = new User
                {
                    UserName = "admin",
                    Role = "Admin" // Assign the Admin role to the user
                };

                var result = userManager.CreateAsync(user, "adminpassword").Result;

                if (result.Succeeded)
                {
                    // You can optionally add more logic here, like sending a confirmation email to the user.
                }
            }
        }
    }
}
