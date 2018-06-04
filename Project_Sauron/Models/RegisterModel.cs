using System.ComponentModel.DataAnnotations;

namespace Project_Sauron.Models
{
    public class RegisterModel
    {
        [Required]
        [MaxLength(20)]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [MaxLength(20)]
        public string Nickname { get; set; }
    }
}