using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Sauron.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name ="Логин")]
        public string Login { get; set; }

        [Required]
        public Guid Password { get; set; }

        [MaxLength(20)]
        [Display(Name ="Никнейм")]
        public string Nickname { get; set; }

        [MaxLength(50)]
        public string Photo { get; set; }

        [Required]
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public List<Enemy> Enemies { get; set; }

        public SiteTheme SiteTheme { get; set; }
    }

    public class Role
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Name { get; set; }
    }
}