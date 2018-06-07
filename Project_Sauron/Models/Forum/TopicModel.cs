using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_Sauron.Models
{
    public class TopicModel
    {
        [Required]
        [Display(Name = "Название темы")]
        public string TopicName { get; set; }

        [Required]
        [Display(Name = "Содержание")]
        public string Text { get; set; }
    }
}