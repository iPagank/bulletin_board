using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace bulletin_board.ViewModels.UserCabinet
{
    public class CreateOrderViewModel
    {
        [Required(ErrorMessage = "Не указано имя")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина имени должна быть от 3 до 50 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Укажите описание")]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "Длина описания должна быть от 3 символов")]
        public string Description { get; set; }

        [Required(ErrorMessage ="Укажите цену")]
        [Range(1, 999999, ErrorMessage = "Недопустимая цена")]
        public double Price { get; set; }

        [Required(ErrorMessage ="Укажите состояние")]
        public bool State { get; set; }

        public IFormFile Path { get; set; }

    }
}