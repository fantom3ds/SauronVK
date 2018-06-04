using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Sauron.Models
{
    public class Enemy
    {
        public int Id { get; set; }
        //Это когда-нибудь закоммитится? Или нет
        [Required]
        [MaxLength(20)]
        public string Link { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(70)]
        public string Photo { get; set; }

        public bool Online { get; set; } = false;

        public long LastActivity { get; set; } = 0;

        public byte Platform { get; set; } = 7;

        [MaxLength(145)]
        public string Status { get; set; }

        [Required]
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public User Author { get; set; }

        public List<EnemyEvent> EnemyEvents { get; set; }

        public override string ToString()
        {
            return Link;
        }
    }
}