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
                    List<UserInfo> Temp = Bound.Users_Info(string.Join(",", DB.Enemies.Select(s => s.Link).ToList()));

                    #region Обновление

                    int i = 0;
                    bool onl = false;
                    foreach (Enemy item in DB.Enemies)
                    {
                        if (item.Name != Temp[i].first_name + " " + Temp[i].last_name)
                            item.Name = Temp[i].first_name + " " + Temp[i].last_name;
                        item.LastActivity = Temp[i].last_seen.time;
                        //Преобразуем byte в bool
                        if (Temp[i].online == 0)
                            onl = false;
                        else
                            onl = true;
                        //Проверяем онлайн
                        if (item.Online != onl)
                        {
                            if (item.Online)
                                DB.EnemyEvents.Add(new EnemyEvent { EnemyId = item.Id, Time = item.LastActivity, EventTypeId = 2 });
                            else
                                DB.EnemyEvents.Add(new EnemyEvent { EnemyId = item.Id, Time = item.LastActivity, EventTypeId = 1 });
                            item.Online = onl;
                        }
                        //Проверяем статус
                        if (item.Status != Temp[i].status)
                        {
                            DB.EnemyEvents.Add(new EnemyEvent { EnemyId = item.Id, Time = item.LastActivity, EventTypeId = 3 });
                            item.Status = Temp[i].status;
                        }
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
