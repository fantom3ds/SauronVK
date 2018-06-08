using Project_Sauron.DataAccesLayer;
using Project_Sauron.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;
using VKAPI;

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

            #region Мой инициализатор
            try
            {
                using (UserContext DB = new UserContext())
                {

                    if (DB.Roles.ToList().Count == 0)
                    {
                        DB.Roles.Add(new Role { Id = 1, Name = "admin" });
                        DB.Roles.Add(new Role { Id = 2, Name = "user" });
                    }
                    if (DB.EventTypes.ToList().Count == 0)
                    {
                        DB.EventTypes.Add(new EventType { Id = 1, Type = "Online" });
                        DB.EventTypes.Add(new EventType { Id = 2, Type = "Online" });
                        DB.EventTypes.Add(new EventType { Id = 3, Type = "StatusChange" });
                    }
                    DB.SaveChanges();

                    if (DB.Users.ToList().Count == 0)
                    {
                        DB.Users.Add(new Models.User
                        {
                            Id = 1,
                            Login = "Admin",
                            Password = Guid.Parse("7066a40f-4277-69cc-4334-7aa96b72931a"),
                            Nickname = "fantom3ds",
                            RegDate = DateTime.Now,
                            RoleId = 1,
                            Status = 1
                        });
                    }
                    DB.SaveChanges();
                }
            }
            catch
            {

            }

            #endregion

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
                    //доработать, въебать селект
                    List<UserInfo> Temp = Bound.Users_Info(string.Join(",", DB.Enemies));

                    //Заккоментирован
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

                    //Включен
                    #region Вариант Верескуна

                    int i = 0;
                    bool onl = false;
                    foreach (Enemy item in DB.Enemies)
                    {
                        item.Name = Temp[i].first_name + " " + Temp[i].last_name;
                        item.LastActivity = Temp[i].last_seen.time;
                        //item.Photo = Temp[i].photo_200_orig;

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
                            Time = 111
                        });
                        DB.SaveChanges();
                    }
                }
                catch { }
            }
        }
    }
}
