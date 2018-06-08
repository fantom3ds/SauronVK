using Project_Sauron.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VKAPI;
using System.Data.Entity;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace Project_Sauron.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //Вполне оптимально работает
        public ActionResult Index()
        {
            //ViewBag.User = new User();
            List <Enemy> enemies2 = new List<Enemy>();
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
        public ActionResult AddEnemy(EnemyModel model)
        {
            try
            {
                string normalID = "";
                if (model.Link.Contains("/"))
                {
                    if (model.Link.LastIndexOf("/") != model.Link.Length - 1)
                        normalID = model.Link.Substring(model.Link.LastIndexOf('/') + 1);
                    else
                        throw new ArgumentException();
                }
                else
                    normalID = model.Link;
                
                VkApi vk = new VkApi();
                var temp = vk.User_Info(normalID);
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
                                AuthorId = thisUser
                            };
                            //Присваиваем онлайн
                            if (temp.online == 0)
                                enemy.Online = false;
                            else
                                enemy.Online = true;

                            DB.Enemies.Add(enemy);
                            DB.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        catch
                        {
                            return RedirectToAction("AddEnemy");
                        }
                    }
                    else
                    {
                        return RedirectToAction("AddEnemy");
                    }
                }//DbContext
            }
            catch(ArgumentNullException)//Если такого пользователя не существует
            {
                return RedirectToAction("AddEnemy");//заглушка
            }
            catch(Exception)
            {
                return RedirectToAction("AddEnemy");
            }
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

                    VkApi vk = new VkApi();
                    var A = new UserInfo();
                    try
                    {
                        A = vk.User_Info(choice.Link);
                    }
                    catch
                    {

                    }
                    ThisEnemy = new ViewEnemyStatistic(choice, A);
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
                principal = DB.Users.Include(r => r.Role).FirstOrDefault(L => L.Login == User.Identity.Name);
                if (principal == null)
                    return
                        new HttpNotFoundResult("Ошибка входа в личный кабинет, перелогиньтесь и попробуйте снова");
                return View(principal);
            }
        }

        public ActionResult EditPrivateOffice(int Id)
        {
            User principal = null;
            using (UserContext DB = new UserContext())
            {
                principal = DB.Users.FirstOrDefault(L => L.Id == Id);
                if (principal == null)
                    return
                        new HttpNotFoundResult("Ошибка входа в личный кабинет, перелогиньтесь и попробуйте снова");
                EditUserModel editUser = new EditUserModel
                {
                    Id = principal.Id,
                    Password = null,
                    Nickname = principal.Nickname,
                    ConfrimPassword = null
                };
                return View(editUser);
            }
        }

        [HttpPost]
        public ActionResult EditPrivateOffice(EditUserModel user)
        {
            User principal = null;
            using (UserContext DB = new UserContext())
            {
                principal = DB.Users.FirstOrDefault(L => L.Id == user.Id);
                if (principal == null)
                    return
                        new HttpNotFoundResult("Ошибка пользователя");

                principal.Nickname = user.Nickname;
                if (user.Password != null && user.ConfrimPassword != null)
                    principal.Password = GetHashString(user.Password);

                DB.SaveChanges();
            }
            return RedirectToAction("PrivateOffice", "Home");
        }

        static Guid GetHashString(string s)
        {
            //переводим строку в байт-массим  
            byte[] bytes = Encoding.Unicode.GetBytes(s);

            //создаем объект для получения средст шифрования  
            MD5CryptoServiceProvider CSP =
                new MD5CryptoServiceProvider();

            //вычисляем хеш-представление в байтах  
            byte[] byteHash = CSP.ComputeHash(bytes);

            string hash = string.Empty;

            //формируем одну цельную строку из массива  
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            return new Guid(hash);
        }
        #endregion
    }
}