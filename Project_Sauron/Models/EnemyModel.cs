using System.ComponentModel.DataAnnotations;

namespace Project_Sauron.Models
{
    public class EnemyModel
    {
        [Required]
        public string Link { get; set; }
    }
}