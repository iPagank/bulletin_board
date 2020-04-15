using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using bulletin_board.Models;
using bulletin_board.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace bulletin_board.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(UserManager<User> user, SignInManager<User> signIn)
        {
            userManager = user;
            signInManager = signIn;
        }

        [HttpGet]
        public IActionResult Register(string returnUrl = null )
        {
            return View(new RegisterViewModel { BackUrl = returnUrl });
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { BackUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { UserName = model.Email, Email = model.Email, PhoneNumber = model.Phone, City = model.City };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    if (!string.IsNullOrEmpty(model.BackUrl) && Url.IsLocalUrl(model.BackUrl))
                    {
                        return Redirect(model.BackUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, item.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.BackUrl) && Url.IsLocalUrl(model.BackUrl))
                    {
                       return Redirect(model.BackUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Неправильный логин или пароль");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Неправильный логин или пароль");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}