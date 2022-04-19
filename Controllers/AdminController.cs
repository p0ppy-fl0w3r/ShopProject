using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop.Models;

namespace MyShop.Controllers
{
    [Authorize(Roles = UserRoles.ADMIN)]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(ApplicationDbContext database, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IUserStore<ApplicationUser> userStore)
        {
            _roleManager = roleManager;
            _userStore = userStore;
            _db = database;
            _userManager = userManager;
        }

        public IActionResult AdminPanel()
        {
            var userList = _db.Users.ToList();

            return View(userList);
        }

        public IActionResult EditUser(string id)
        {

            var user = _db.Users.SingleOrDefault(u => u.Id == id);

            var oldUserModel = new OldUserModel
            {
                UserId = id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                IsNewPassword = false
            };

            return View(oldUserModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(OldUserModel model)
        {

            if (!model.IsNewPassword)
            {
                ModelState.Remove("ConfirmPassword");
                ModelState.Remove("Password");
            }

            if (ModelState.IsValid)
            {
                var mUser = _db.Users.SingleOrDefault(u => u.Id == model.UserId);

                mUser.FirstName = model.FirstName;
                mUser.LastName = model.LastName;
                mUser.DateOfBirth = model.DateOfBirth;
                mUser.Gender = model.Gender;
                if (model.IsNewPassword)
                {
                    var passwordHash = _userManager.PasswordHasher.HashPassword(mUser, model.Password);
                    mUser.PasswordHash = passwordHash;
                }

                await _userManager.UpdateAsync(mUser);

                return RedirectToAction(nameof(AdminPanel));
            }
            else
            {
                return View();
            }
        }


        public IActionResult AddNew()
        {

            return View(new NewUserModel());
        }

        public async Task<IActionResult> RegisterNewUser(NewUserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = Activator.CreateInstance<ApplicationUser>();

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Gender = model.Gender;
                user.DateOfBirth = model.DateOfBirth;

                await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Assumes that superuser(ADMIN) is already created.
                    if (!await _roleManager.RoleExistsAsync(UserRoles.ASSISTANT))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(UserRoles.ASSISTANT));
                    }
                    return RedirectToAction(nameof(AdminPanel));
                }
                return BadRequest();

            }
            return RedirectToAction(nameof(AdminPanel));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = _db.Users.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                // Only delete non admin users
                if (!await _userManager.IsInRoleAsync(user, UserRoles.ADMIN))
                {
                    _db.Users.Remove(user);
                }
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(AdminPanel));
        }
    }
}
