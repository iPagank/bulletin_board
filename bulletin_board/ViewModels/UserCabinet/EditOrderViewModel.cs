using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace bulletin_board.ViewModels.UserCabinet
{
    public class EditOrderViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указано имя")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина имени должна быть от 3 до 50 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Укажите описание")]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "Длина описания должна быть от 3 символов")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Укажите цену")]
        [Range(1,999999, ErrorMessage = "Недопустимая цена")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Укажите состояние")]
        public bool State { get; set; }

        public IFormFile Path { get; set; }

        public string Absolute_path { get; set; }


    }
}
