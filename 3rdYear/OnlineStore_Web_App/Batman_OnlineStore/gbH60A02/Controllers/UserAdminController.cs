using gbH60A02.Areas.Identity.Models;
using gbH60A02.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gbH60A02.Controllers
{
    [Authorize(Roles = "Manager")]
    public class UserAdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICustomerRepository _custRepo;

        public UserAdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ICustomerRepository custRepo)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _custRepo = custRepo;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }


        public IActionResult Create()
        {
            ViewData["Roles"] = _roleManager.Roles.Where(x => !x.Name.Equals("Customer")).Select(r => r.Name).ToList();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            ViewData["Roles"] = _roleManager.Roles.Where(x => !x.Name.Equals("Customer")).Select(r => r.Name).ToList();
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.SelectedRole))
                    {
                        await _userManager.AddToRoleAsync(user, model.SelectedRole);
                    }

                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }


        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.Where(x => !x.Name.Equals("Customer")).Select(r => r.Name).ToList();

            var model = new EditUserViewModel
            {
                User = user,
                UserRole = (userRoles.FirstOrDefault() ?? "Staff"),
                AvailableRoles = allRoles
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(string id, EditUserViewModel model, string selectedRole = "Customer")
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var cust = await _custRepo.FindByEmail(user.Email);

            if (cust != null)
            {
                selectedRole = "Customer";
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, userRoles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return View(model);
            }

            result = await _userManager.AddToRolesAsync(user, [selectedRole]);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add selected roles");
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

                if (!passwordResult.Succeeded)
                {
                    foreach (var error in passwordResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }

            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ViewData["Role"] = ((await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "Customer");
            return View(user);
        }


        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var cust = await _custRepo.FindByEmail(user.Email);

            if (cust != null)
            {
                try
                {
                    await _custRepo.Remove(cust.CustomerId);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Failed to delete user");
                    return View(user);
                }
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Failed to delete user");
                return View(user);
            }

            return RedirectToAction("Index");
        }
    }

}
