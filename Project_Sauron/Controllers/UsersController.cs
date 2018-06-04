using Project_Sauron.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Project_Sauron.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            using (UserContext Allusers = new UserContext())
            {
                List<User> users = new List<User>(Allusers.Users.Where(x => x.Role.Name == "user"));
                return View(users);
            }
        }

        #region Админская регистрация нового пользователя

        public ActionResult RegisterNewUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterNewUser(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (UserContext db = new UserContext())
                {
                    user = db.Users.FirstOrDefault(u => u.Login == model.Login);

                    if (user == null)
                    {
                        // создаем нового пользователя
                        db.Users.Add(new User { Login = model.Login, Password = GetHashString(model.Password), Nickname = model.Nickname, Role = db.Roles.ToList()[1] });
                        db.SaveChanges();

                        Guid password = GetHashString(model.Password);
                        user = db.Users.Where(u => u.Login == model.Login && u.Password == password).FirstOrDefault();

                        // если пользователь удачно добавлен в бд
                        if (user != null)
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                    }
                }
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Удаление зарегистрированного пользователя из базы (доработать!)

        public ActionResult DeleteUser(int Id)
        {
            User principal = null;
            List<Enemy> enemies = null;

            using (UserContext user = new UserContext())
            {
                principal = user.Users.FirstOrDefault(x => x.Id == Id);
                if (principal == null)
                    return HttpNotFound();
                enemies = user.Enemies.Where(y => y.Author.Id == principal.Id).ToList();
                principal.Enemies = enemies;
            }
            return View(principal);
        }

        [HttpPost, ActionName("DeleteUser")]
        public ActionResult DeleteConfirmed(int Id)
        {
            User principal = null;
            using (UserContext context = new UserContext())
            {
                principal = context.Users.FirstOrDefault(x => x.Id == Id);
                if (principal == null)
                    return HttpNotFound();

                context.Entry(principal).Collection(c => c.Enemies).Load();
                context.Users.Remove(principal);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        #endregion

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

        #region Отображение данных пользователя

        public ActionResult UserDetails(int Id)
        {
            User principal = null;
            using (UserContext DB = new UserContext())
            {
                principal = DB.Users.FirstOrDefault(x => x.Id == Id);
                if (principal == null)
                    return HttpNotFound();
                principal.Enemies = DB.Enemies.Where(y => y.Author.Id == principal.Id).ToList();
            }
            return View(principal);
        }

        #endregion

        #region Редактирование данных пользователя админом (сделать что-то с паролем)

        public ActionResult EditUser(int Id)
        {
            User principal = null;
            using (UserContext DB = new UserContext())
            {
                principal = DB.Users.FirstOrDefault(x => x.Id == Id);
                if (principal == null)
                    return HttpNotFound();
                principal.Enemies = DB.Enemies.Where(y => y.Author.Id == principal.Id).ToList();
            }
            return View(principal);
        }

        [HttpPost]
        public ActionResult EditUser(User user)
        {
            using (UserContext DB = new UserContext())
            {
                if (user != null)
                {
                    User oldUser = DB.Users.FirstOrDefault(u => u.Id == user.Id);

                    oldUser.Login = user.Login;
                    oldUser.Password = user.Password;
                    oldUser.Nickname = user.Nickname;

                    DB.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        #endregion
    }

}