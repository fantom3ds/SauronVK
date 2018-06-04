using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_Sauron.Models
{
    public class SiteTheme
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string ThemeName { get; set; }

        [Required]
        [MaxLength(50)]
        public string CssPath { get; set; }
    }
}