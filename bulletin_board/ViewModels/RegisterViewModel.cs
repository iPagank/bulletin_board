using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace bulletin_board.ViewModels
{
    public class RegisterViewModel
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string City { get; set; }

        public string Password { get; set; }

        public string ConfirmPass { get; set; }
    }
}
