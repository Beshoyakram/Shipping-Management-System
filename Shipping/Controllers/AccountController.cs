using Shipping.Models;
using Shipping.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Shipping.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            //Check if there is account with the same email
            var res = await _userManager.FindByEmailAsync(registerVM.Email);
            if (res != null)
            { ModelState.AddModelError("", "Account already exist"); }

            if (ModelState.IsValid)
            {
                ApplicationUser newUser = new ApplicationUser();
                newUser.Address = registerVM.Address;
                newUser.PhoneNumber = registerVM.PhoneNumber;
                newUser.Email = registerVM.Email;
                newUser.UserName = registerVM.UserName;
                newUser.PasswordHash = registerVM.Password;

                var result = await _userManager.CreateAsync(newUser, registerVM.Password);
                if (result.Succeeded)
                {
                    //Assign to user role
                    await _userManager.AddToRoleAsync(newUser, "User");

                    //create cookie
                    await _signInManager.SignInAsync(newUser, isPersistent: false);
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                }
            }
            return View(registerVM);
        }
        #endregion


        #region Login
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "home");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel LoginVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(LoginVM.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "UserName or Password is wrong");
                }
                else
                {
                    var result = await _signInManager.PasswordSignInAsync(user, LoginVM.Password, LoginVM.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    { ModelState.AddModelError("", "UserName or Password is wrong"); }
                }
            }
            return View();
        }
        #endregion

        #region Logout
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        #endregion


        #region Change Password
        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }
                await _signInManager.SignOutAsync();

                return View("ChangePasswordConfirmation");
            }
            return View(model);
        }
        #endregion
    }
}
