using Project_Sauron.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using VKAPI;
using xNet;

namespace Project_Sauron
{
    public class MvcApplication : System.Web.HttpApplication
    {
        Timer RefreshTimer;
        TimerCallback RefreshCallback;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            RefreshCallback = new TimerCallback(RefreshTimer_Tick);
            RefreshTimer = new Timer(RefreshCallback, null, 0, 10000);
        }

        private static void RefreshTimer_Tick(object state)
        {
            try
            {
                using (UserContext DB = new UserContext())
                {
                    VkApi Bound = new VkApi();
                    List<UserInfo> Temp = Bound.Users_Info(string.Join(",", DB.Enemies));

                    //Включен
                    #region Мой вариант

                    ////Вытаскиваем все данные из БД
                    //List<Enemy> AllEnemies = new List<Enemy>(DB.Enemies); //+1
                    //List<EnemyEvent> AllEvents = new List<EnemyEvent>(DB.EnemyEvents);

                    ////Удаляем всех из базы
                    //DB.Enemies.RemoveRange(AllEnemies);
                    //DB.SaveChanges();//+1

                    //foreach (var item in AllEnemies)
                    //{
                    //    //Присваиваем каждому врагу список его событий
                    //    item.EnemyEvents = AllEvents.Where(L => L.EnemyId == item.Id).ToList();
                    //    //Начинаем обновлять данные
                    //    item.Name = Temp[i].first_name + " " + Temp[i].last_name;
                    //    item.LastActivity = Temp[i].last_seen.time;
                    //    item.Photo = Temp[i].photo_200_orig;
                    //    if (Temp[i].online == 0)
                    //        onl = false;
                    //    else
                    //        onl = true;
                    //    if (item.Online != onl)
                    //    {
                    //        if (item.Online)
                    //            item.EnemyEvents.Add(new EnemyEvent { Enemy = item, Time = item.LastActivity, EventTypeId = 2 });
                    //        else
                    //            item.EnemyEvents.Add(new EnemyEvent { Enemy = item, Time = item.LastActivity, EventTypeId = 1 });
                    //    }
                    //    item.Online = onl;
                    //    if (item.Status != Temp[i].status)
                    //    {
                    //        item.EnemyEvents.Add(new EnemyEvent { Enemy = item, Time = item.LastActivity, EventTypeId = 3 });
                    //    }
                    //    item.Status = Temp[i].status;
                    //    i++;
                    //    item.Id = new int();
                    //}
                    //DB.Enemies.AddRange(AllEnemies);
                    //DB.SaveChanges();//+1

                    #endregion

                    //Закомментирован
                    #region Вариант Верескуна

                    int i = 0;
                    bool onl = false;
                    foreach (var item in DB.Enemies)
                    {
                        item.Name = Temp[i].first_name + " " + Temp[i].last_name;
                        item.LastActivity = Temp[i].last_seen.time;
                        item.Photo = Temp[i].photo_200_orig;

                        if (Temp[i].online == 0)
                            onl = false;
                        else
                            onl = true;
                        if (item.Online != onl)
                        {
                            if (item.Online)
                                DB.EnemyEvents.Add(new EnemyEvent { EnemyId = item.Id, Time = item.LastActivity, EventTypeId = 2 });
                            else
                                DB.EnemyEvents.Add(new EnemyEvent { EnemyId = item.Id, Time = item.LastActivity, EventTypeId = 1 });
                        }
                        item.Online = onl;
                        if (item.Status != Temp[i].status)
                        {
                            DB.EnemyEvents.Add(new EnemyEvent { EnemyId = item.Id, Time = item.LastActivity, EventTypeId = 3 });
                        }
                        item.Status = Temp[i].status;
                        i++;
                    }

                    DB.SaveChanges();

                    #endregion

                }//DBContext
            }
            catch (Exception Ex)
            {
                try
                {
                    using (UserContext DB = new UserContext())
                    {
                        DB.Errors.Add(new Error
                        {
                            Message = Ex.Message,
                            TargetSite = Ex.TargetSite.ToString(),
                            Time = (long)((DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds)
                        });
                        DB.SaveChanges();
                    }
                }
                catch { }
            }
        }


    }
}
