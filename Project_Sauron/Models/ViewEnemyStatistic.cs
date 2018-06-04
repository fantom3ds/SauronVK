using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VKAPI;

namespace Project_Sauron.Models
{
    public class ViewEnemyStatistic
    {
        public int Id { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Онлайн-статус")]
        public string Online { get; set; }

        [Display(Name = "Последняя активность")]
        public string LastActivity { get; set; }

        [Display(Name = "Статус")]
        public string Status { get; set; }

        public string Photo { get; set; }

        [Display(Name = "Статистика")]
        public List<ViewEvent> ViewEvents { get; set; }

        public ViewEnemyStatistic(Enemy enemy, UserInfo info)
        {
            Id = enemy.Id;
            Name = enemy.Name;

            if (enemy.Online)
                Online = "В сети";
            else
                Online = "Не в сети";

            string platform = "(Полная версия сайта)";
            switch (enemy.Platform)
            {
                case 1: platform = "(мобильной версия)"; break;
                case 2: platform = "(iPhone)"; break;
                case 3: platform = "(iPad)"; break;
                case 4: platform = "(Android)"; break;
                case 5: platform = "(Windows Phone)"; break;
                case 6: platform = "(Приложение Windows 10)"; break;
                case 7: platform = "(Полная версия сайта)"; break;
                case 8: platform = "(VK Mobile)"; break;
                default: platform = ""; break;
            }
            LastActivity = (new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(enemy.LastActivity).ToLocalTime()).ToString() + " " + platform;

            Status = enemy.Status;
            Photo = info.photo_200_orig;

            ViewEvents = new List<ViewEvent>();
            foreach (var item in enemy.EnemyEvents)
            {
                ViewEvents.Add(new ViewEvent(item));
            }
        }
    }
}