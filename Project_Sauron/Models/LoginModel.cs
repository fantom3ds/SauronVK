using System.ComponentModel.DataAnnotations;

namespace Project_Sauron.Models
{
    public class LoginModel
    {
        [Required]
        [MaxLength(20)]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}