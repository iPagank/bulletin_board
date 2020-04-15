using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace bulletin_board.ViewModels.UserCabinet
{
    public class AccountSettingViewModel
    {
        [Required(ErrorMessage = "Не указано имя")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина имени должна быть от 3 до 50 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не указан Email")]
        [EmailAddress(ErrorMessage ="Неккоректный Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан Телефон")]
        [Phone(ErrorMessage = "Неккоректный Телефон")]
        public string Phone { get; set; }

    }
}
