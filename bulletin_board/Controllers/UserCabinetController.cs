using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bulletin_board.Models;
using bulletin_board.ViewModels.UserCabinet;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace bulletin_board.Controllers
{
    [Authorize]
    public class UserCabinetController : Controller
    {
        private ApplicationContext db;

        private UserManager<User> userManager;

        private IWebHostEnvironment appEnvironment;
        public UserCabinetController( ApplicationContext context, UserManager<User> user, IWebHostEnvironment webHostEnvironment)
        {
            db = context;
            userManager = user;
            appEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {

            var user = await userManager.GetUserAsync(User);
            
            if (user != null)
            {
                var orders = db.Products.Where(n => n.User.Id == user.Id).OrderByDescending(p => p.Id).ToList();
            ViewBag.orders = orders;

            
                AccountSettingViewModel model = new AccountSettingViewModel { Email = user.Email, Name = user.Name, Phone = user.PhoneNumber };
                return View("Index", model);
            }
            
            return  View();

        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderViewModel model)
        { 
          if (ModelState.IsValid)
          {
                var user = await userManager.GetUserAsync(User);
                
                if(user != null)
                {

                    if(model.Path != null)
                    {
                        string path = "/avatars/" + model.Path.FileName;

                        using(var fileStream = new FileStream(appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await model.Path.CopyToAsync(fileStream);
                        }
                        var product = new Product { Name = model.Name, Description = model.Description, Price = model.Price, State = model.State, Path = path, User = user };
                        
                        db.Products.Add(product);
                        await db.SaveChangesAsync();

                        user.Products.Add(product);
                        var user_result = await userManager.UpdateAsync(user);

                        if (user_result.Succeeded)
                        {
                            return RedirectToAction("Index", "UserCabinet");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Ощибка сервера, попробуйте черех нескольно минут");
                        }
                    }
                }
          }
           else
           {
              ModelState.AddModelError("", "Ощибка валидации");
            return View(model);
           }
           return View(model);
        }

        public IActionResult Edit(int? id)
        {
            if (id != 0)
            {
                var order = db.Products.FirstOrDefault(p => p.Id == id);

                if(order != null)
                {
                    EditOrderViewModel viewModel = new EditOrderViewModel {Id = order.Id, Name = order.Name, Description = order.Description, Price = order.Price, State = order.State , Absolute_path = order.Path };
                    return View(viewModel);
                }
            }
            
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                Product product = await db.Products.FirstOrDefaultAsync(p => p.Id == model.Id);
                if(product != null)
                {
                    string newPath = "";
                    if (model.Path != null)
                    {
                         newPath = "/img/" + model.Path.FileName;
                    }
                    
                    if ( newPath != product.Path || product.Path == null && newPath != "")
                    {
                           
                        using(FileStream file = new FileStream(appEnvironment.WebRootPath + newPath, FileMode.Create))
                        {
                            await model.Path.CopyToAsync(file);
                        }

                        if(product.Path != null)
                        {
                            System.IO.File.Delete(appEnvironment.WebRootPath + product.Path);
                        }
                        
                        product.Name = model.Name;
                        product.Description = model.Description;
                        product.Price = model.Price;
                        product.State = model.State;
                        product.Path = newPath;

                        db.Products.Update(product);
                        await db.SaveChangesAsync();

                        return RedirectToAction("Index", "UserCabinet");
                    }
                    else
                    {
                        product.Name = model.Name;
                        product.Description = model.Description;
                        product.Price = model.Price;
                        product.State = model.State;

                        db.Products.Update(product);
                        await db.SaveChangesAsync();

                        return RedirectToAction("Index", "UserCabinet");
                    }
                    
                }
                
            }
            else
            {
                ModelState.AddModelError("", "Model Error!");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id != null)
            {
                
                Product product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);
                if(product != null)
                {
                    System.IO.File.Delete(appEnvironment.WebRootPath + product.Path);
                db.Entry(product).State = EntityState.Deleted;
                await db.SaveChangesAsync();

                return RedirectToAction("Index", "UserCabinet");
                }
                
            }
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> EditUser(AccountSettingViewModel model)
        {
            if (ModelState.IsValid)
            {

                User user = await userManager.GetUserAsync(User);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.Name = model.Name;
                    user.PhoneNumber = model.Phone;

                    var res = await userManager.UpdateAsync(user);
                    if (res.Succeeded)
                    {
                        return RedirectToAction("Index", "UserCabinet");
                    }
                    ModelState.AddModelError("", "UpdateError");
                }
            }
            else
            {
                ModelState.AddModelError("", "ModelError");
            }
            return NotFound();
        }
    }
}