using System.ComponentModel.DataAnnotations;

namespace Project_Sauron.Models
{
    public class Error
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Display(Name = "Сообщение")]
        public string Message { get; set; }

        [MaxLength(30)]
        [Display(Name = "Метод, вызвавший ошибку")]
        public string TargetSite { get; set; }

        [Display(Name ="Время")]
        public long Time { get; set; }
    }
}