using Microsoft.AspNetCore.Identity;
using MyShop.Models;

namespace MyShop
{
    public class AddData
    {

        public async Task CreateSuperAdmin(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            IdentityResult roleResult;

            var roleNames = new List<String>
    {
        UserRoles.ADMIN,
        UserRoles.ASSISTANT
    };

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {

                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }


            var superUser = new ApplicationUser
            {
                FirstName = "Mohit",
                LastName = "Shrohi",
                DateOfBirth = DateTime.Parse("1998/01/16"),
                Gender = "Male",
                UserName = "Mohit@gmail.com",
                Email = "Mohit@gmail.com",
            };

            string superPassword = "Mypassword1!";
            var _user = await UserManager.FindByEmailAsync("Mohit@gmail.com");

            if (_user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(superUser, superPassword);
                if (createPowerUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(superUser, UserRoles.ADMIN);

                }
            }
        }
    }
}
