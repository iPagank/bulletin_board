using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using bulletin_board.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace bulletin_board.Controllers
{
    public class HomeController : Controller
    {
        ApplicationContext db;

        UserManager<User> manager;

        public HomeController(ApplicationContext context, UserManager<User> _manager)
        {
            db = context;
            manager = _manager;
        }

       public IActionResult Index()
        {
           return View(db.Products.ToList());
        }
        [Authorize]
       public async Task<IActionResult> Product(int? id)
        {
            if(id != null)
            {
                var result = await db.Products.Include(p=>p.User).FirstOrDefaultAsync(p => p.Id == id);

                if(result != null)
                {
                    return View(result);
                }
            }
            return NotFound();
        }
        [Authorize]
        public async Task<IActionResult> Profile(string id)
        {
            if (!string.IsNullOrEmpty(id)) 
            {
                User user = await db.Users.Include(p => p.Products).FirstOrDefaultAsync(p => p.Id == id);
                if(user != null)
                {
                    return View(user);
                }
            }
            return NotFound();
        }
    }
}
