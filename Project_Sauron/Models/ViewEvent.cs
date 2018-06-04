using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_Sauron.Models
{
    public class ViewEvent
    {
        public int Id { get; set; }

        [Display(Name = "Событие")]
        public string Event { get; set; }

        [Display(Name = "Дата и время")]
        public DateTime Time { get; set; }

        public ViewEvent(EnemyEvent enemyEvent)
        {
            Id = enemyEvent.Id;
            Event = enemyEvent.Type.Type + " <" + enemyEvent.SetStatus+">";
            Time = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(enemyEvent.Time);
        }
    }
}