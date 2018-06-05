using System.ComponentModel.DataAnnotations;

namespace Project_Sauron.Models
{
    public class RegisterModel
    {
        [Display(Name = "Страница ВК (для фото и информации о пользователе, не бойтесь)")]
        public string PageVK { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 3)]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Повтор пароля")]
        [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        public string ConfrimPassword { get; set; }

        
        [Display(Name = "Никнейм")]
        public string Nickname { get; set; }
    }
}