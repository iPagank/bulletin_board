using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bulletin_board.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool State { get; set; }
        public string Path { get; set; }

        public User User { get; set; }
    }
}
