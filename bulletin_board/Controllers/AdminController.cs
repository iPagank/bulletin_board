using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using bulletin_board.Models;
using bulletin_board.ViewModels.Admin;
using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace bulletin_board.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        RoleManager<IdentityRole> roleManager;
        UserManager<User> userManager;
        ApplicationContext db;

        public AdminController(RoleManager<IdentityRole> _roleManager, UserManager<User> _userManager, ApplicationContext context)
        {
            roleManager = _roleManager;
            userManager = _userManager;
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RoleList()
        {
            return View(roleManager.Roles.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(name);
        }

        public IActionResult UserList() 
        {
           return View(userManager.Users.ToList());
        }

        public async Task<IActionResult> Edit(string userId)
        {
            User user = await userManager.FindByIdAsync(userId);

            if(user != null)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var allRoles = roleManager.Roles.ToList();

                GhangeRoleViewModel result = new GhangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                 return View(result);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            User user = await userManager.FindByIdAsync(userId);
            if(user != null) 
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var allRoles = roleManager.Roles.ToList();
                var addedRoles = roles.Except(userRoles);
                var removedRoles = userRoles.Except(roles);

                await userManager.AddToRolesAsync(user, addedRoles);
                await userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("UserList");
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);

            if(role != null)
            {
                IdentityResult result = await roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            IdentityUser user = await userManager.FindByIdAsync(id);
            if(user != null)
            {
               IEnumerable <Product> products = db.Products.Where(p => p.User.Id == user.Id).ToList();
                if(products != null)
                {
                    db.Products.RemoveRange(products);
                    await db.SaveChangesAsync();

                    return RedirectToAction("UserList", "Admin");
                }
            }
            return RedirectToAction("UserList", "Admin");
        }
    }
}