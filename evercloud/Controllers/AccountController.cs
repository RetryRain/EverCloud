using evercloud.Domain.Interfaces;
using evercloud.ViewModels;
using evercloud.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace evercloud.Controllers
{
    public class AccountController(SignInManager<Users> signInManager, IAccountService accountService) : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Email or password is incorrect.");
            }

            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Users user = new ()
                {
                    FullName = model.Name,
                    Email = model.Email,
                    UserName = model.Email,
                };

                bool success = await accountService.RegisterUserAsync(user, model.Password);

                if (success)
                    return RedirectToAction("Login", "Account");

                ModelState.AddModelError("", "Failed to create account.");
            }

            return View(model);
        }

        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await accountService.FindByUsernameAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("", "Something is wrong!");
                }
                else
                {
                    return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
                }
            }

            return View(model);
        }

        public IActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("VerifyEmail", "Account");
            }

            return View(new ChangePasswordViewModel { Email = username });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await accountService.FindByUsernameAsync(model.Email);
                if (user != null)
                {
                    bool result = await accountService.ResetPasswordAsync(user, model.NewPassword);

                    if (result)
                        return RedirectToAction("Login", "Account");

                    ModelState.AddModelError("", "Failed to reset password.");
                }
                else
                {
                    ModelState.AddModelError("", "Email not found!");
                }
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong. try again.");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount()
        {
            var user = await signInManager.UserManager.GetUserAsync(User);

            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction("Dashboard", "Account");
            }

            bool result = await accountService.DeleteAccountAsync(user);

            if (result)
            {
                await signInManager.SignOutAsync();
                TempData["Message"] = "Your account has been deleted successfully.";
                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "Failed to delete the account.";
            return RedirectToAction("Dashboard", "Account");
        }
    }
}
