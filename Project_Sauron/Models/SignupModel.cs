using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_Sauron.Models
{
    // Перенести в RegisterModel !!! Катюха, ты меня слышишь? Сделано норм, но НЕ ТАМ.
    // И контроллер не Users, а Account!
    public class SignupModel
    {
            [Required]
            [EmailAddress]
            [Display(Name = "Почта")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 3)]
            [Display(Name = "Логин")]
            public string Username { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Повтор пароля")]
            [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
            public string ConfrimPassword { get; set; }


    }
}