using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace bulletin_board.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string City { get; set; }
        public List<Product> Products { get; set; }
        public string Path { get; set; }

        public User()
        {
            Products = new List<Product>();
        }
    }
}
