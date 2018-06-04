using AutoMapper;
using Project_Sauron.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VKAPI;

using System.Data.Entity;
using System.Threading.Tasks;

namespace Project_Sauron.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //Вполне оптимально работает
        public ActionResult Index()
        {
            List<Enemy> enemies2 = new List<Enemy>();
            try
            {
                using (UserContext DB = new UserContext())
                {
                    enemies2 = new List<Enemy>(DB.Enemies.Where(x => x.Author.Login == User.Identity.Name));
                }

                List<ViewEnemy> views = new List<ViewEnemy>();
                for (int i = 0; i < enemies2.Count; i++)
                {
                    views.Add(new ViewEnemy(enemies2[i]));
                }
                enemies2.Clear();

                return View(views);
            }
            catch (Exception Ex)
            {
                return new HttpNotFoundResult(Ex.Message + " | " + Ex.TargetSite);
            }
        }

        #region Добавление врага в список

        public ActionResult AddEnemy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddEnemy(string link)
        {
            try
            {
                VkApi vk = new VkApi();
                var temp = vk.User_Info(link);
                Enemy enemy = null;

                using (UserContext DB = new UserContext())
                {
                    //Находим текущего юзера в базе, получаем его ключ
                    int thisUser = DB.Users.Where(t => t.Login == User.Identity.Name).Select(i => i.Id).FirstOrDefault();
                    //Ищем в коллекции похожего
                    enemy = DB.Enemies.Where(id => id.AuthorId == thisUser).FirstOrDefault(ii => ii.Link == temp.id);
                    //Если такого нет, то добавляем
                    if (enemy == null)
                    {
                        try
                        {
                            enemy = new Enemy
                            {
                                Link = temp.id,
                                Name = temp.first_name + " " + temp.last_name,
                                LastActivity = temp.last_seen.time,
                                Status = temp.status,
                                Photo = temp.photo_200_orig,
                                AuthorId = thisUser
                            };
                            //Присваиваем онлайн
                            if (temp.online == 0)
                                enemy.Online = false;
                            else
                                enemy.Online = true;

                            DB.Enemies.Add(enemy);
                            DB.SaveChanges();
                        }
                        catch (Exception Ex)
                        {
                            ModelState.AddModelError("", Ex.Message + " | " + Ex.TargetSite);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Такой пользователь уже существует");
                    }
                }//DbContext
            }
            catch
            {

            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Удаление врага

        //Для подтверждения (окно с информацией)
        public ActionResult DeleteEnemy(int Id)
        {
            using (UserContext DB = new UserContext())
            {
                Enemy removing = DB.Enemies.FirstOrDefault(n => n.Id == Id);
                if (removing != null)
                {
                    return View(removing);
                }
                else
                    return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult DeleteEnemy(Enemy enemy)
        {
            using (UserContext DB = new UserContext())
            {
                Enemy removing = DB.Enemies.FirstOrDefault(n => n.Id == enemy.Id);
                if (removing != null)
                {
                    DB.Enemies.Remove(removing);
                    DB.SaveChanges();
                }
                else
                    return HttpNotFound();
            }

            return RedirectToAction("Index");
        }

        #endregion

        //3 запроса к БД, оптимизировать нельзя
        public ActionResult EnemyStatistic(int Id)
        {
            Enemy choice = null;
            ViewEnemyStatistic ThisEnemy = null;
            try
            {
                using (UserContext DB = new UserContext())
                {
                    choice = DB.Enemies.FirstOrDefault(c => c.Id == Id);//+1
                    if (choice == null)
                        return RedirectToAction("Index");
                    DB.Entry(choice).Collection(u => u.EnemyEvents).Load();//+2

                    ThisEnemy = new ViewEnemyStatistic(choice);
                }
                return View(ThisEnemy);
            }
            catch (DivideByZeroException)
            {
                return RedirectToAction("Index");
            }
        }

        #region Личный кабинет

        //Отображение инфы
        public ActionResult PrivateOffice()
        {
            User principal = null;
            using (UserContext DB = new UserContext())
            {
                principal = DB.Users.Include(r => r.Role).Include(th => th.SiteTheme).FirstOrDefault(L => L.Login == User.Identity.Name);
                if (principal == null)
                    return
                        new HttpNotFoundResult("Ошибка входа в личный кабинет, перелогиньтесь и попробуйте снова");
                return View(principal);
            }
        }

        #endregion
    }
}