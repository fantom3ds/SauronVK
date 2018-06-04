using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Sauron.Models
{
    public class EnemyEvent
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Enemy")]
        public int EnemyId { get; set; }

        [Required]
        public Enemy Enemy { get; set; }

        [Required]
        [ForeignKey("Type")]
        public int EventTypeId { get; set; }
        public virtual EventType Type { get; set; }

        [Required]
        public long Time { get; set; } = 0;

    }

    public class EventType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Type { get; set; }
    }
}