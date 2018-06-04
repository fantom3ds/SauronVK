using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public List<User> Users { get; set; }
    }
}