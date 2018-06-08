using System;
using System.ComponentModel.DataAnnotations;

namespace Project_Sauron.Models
{
    public class ViewEnemy
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

        public ViewEnemy(Enemy enemy)
        {
            string onl;
            string platform = "(Полная версия сайта)";

            if (enemy.Online)
                onl = "В сети";
            else
                onl = "Не в сети";

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

            Id = enemy.Id;
            Name = enemy.Name;
            Online = onl;
            LastActivity = (new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(enemy.LastActivity).ToLocalTime()).ToString() + " " + platform;
            Status = enemy.Status;
            //Photo = enemy.Photo;
        }

    }
}