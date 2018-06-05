using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_Sauron.Models
{
    public class ViewError
    {
        public int Id { get; set; }

        [Display(Name = "Сообщение")]
        public string Message { get; set; }
        
        [Display(Name = "Метод, вызвавший ошибку")]
        public string TargetSite { get; set; }

        [Display(Name = "Время")]
        public DateTime Time { get; set; }

        public ViewError(Error error)
        {
            Id = error.Id;
            Message = error.Message;
            TargetSite = error.TargetSite;
            Time = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(error.Time).ToLocalTime();
        }

        public static implicit operator ViewError(Error error)
        {
            return new ViewError(error);
        }
    }
}