using Project_Sauron.Logic;
using Project_Sauron.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Project_Sauron.Controllers
{
    public class AccountController : Controller
    {
        ForumLogic flogic = new ForumLogic();
        UserLogic ulogic = new UserLogic();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // поиск пользователя в бд
                    User user = null;
                    using (UserContext db = new UserContext())
                    {
                        Guid password = GetHashString(model.Password);
                        user = db.Users.FirstOrDefault(u => u.Login == model.Login && u.Password == password);
                        if (user != null)
                        {
                            FormsAuthentication.SetAuthCookie(model.Login, true);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                        }
                    }
                }
                return View(model);
            }
            catch(Exception Ex)
            {
                return new HttpNotFoundResult(Ex.Message + "\n" + Ex.TargetSite + "\n" + Ex.InnerException);
            }
        }

        //==============================Регистрация (пока закрыта)=======================================
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
//                    User user = null;
//                    using (UserContext db = new UserContext())
//                    {
//                        user = db.Users.FirstOrDefault(u => u.Login == model.Login);

//                        if (user == null)
//                        {
//                            // создаем нового пользователя 
//                            db.Users.Add(new User { Login = model.Login, Password = GetHashString(model.Password), Nickname = model.Nickname, Role = db.Roles.ToList()[1] });
//                            db.SaveChanges();

//                            Guid password = GetHashString(model.Password);
//                            user = db.Users.Where(u => u.Login == model.Login && u.Password == password).FirstOrDefault();

//                            // если пользователь удачно добавлен в бд 
//                            if (user != null)
//                            {
//                                FormsAuthentication.SetAuthCookie(model.Login, true);
//                                return RedirectToAction("Index", "Home");
//                            }
//                        }
//                        else
//                        {
//                            ModelState.AddModelError("", "Пользователь с таким логином уже существует");
//                        }
//                    }
//                }
                if (!ulogic.CheckUserReg(model.Nickname))
                {
                    MailAddress from = new MailAddress("catrindas@mail.com", "Registration");
                    MailAddress to = new MailAddress(model.Email);
                    MailMessage msg = new MailMessage(from, to)
                    {
                        Subject = "Подтверждение регистрации",
                        Body = string.Format("Для завершения регистрации перейдите по ссылке: " + "<a href=\"{0}\" title=\"Подтвердить регистрацию\">{0}</a>", Url.Action("ConfirmEmail", "User", new { userName = model.Nickname, email = model.Email }, Request.Url.Scheme)),
                        IsBodyHtml = true
                    };
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
                    {
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential("catrindas@mail.com", "Катюня"),
                        EnableSsl = true
                    };
                    smtp.Send(msg);
                    ulogic.AddUser(model.Nickname, model.Password, model.Email);
                    /* FormsAuthentication.SetAuthCookie(model.Username, false);
                     return RedirectToAction("Index", "Home");*/
                    return RedirectToAction("Confirm", "User", new { email = model.Email });
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
            }        
            return View(model);
        }

        public string Confirm(string email)
        {
            return "На почтовый адрес " + email + " были высланы дальнейшие инструкции по завершению регистрации";
        }
        public ActionResult ConfirmEmail(string Nickname, string email)
        {
            ulogic.ConfirmEmail(Nickname);
            return RedirectToAction("Login", "User");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult PrivateOffice(string Nickname)
        {
            ViewBag.Time = flogic.GetUserTime(Nickname);
            return View(ulogic.GetUserByLogin(Nickname));
        }

        public ActionResult UserActivity(string Nickname, string email)
        {
            ViewBag.Email = email;
            return PartialView("_UserActivityPartial", ulogic.GetUserComments(Nickname));
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
    }
}