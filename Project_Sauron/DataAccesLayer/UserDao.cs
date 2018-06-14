using Project_Sauron.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Project_Sauron.DataAccesLayer
{
    public class UserDao 
    {      
        public void Add(User user)
        {
            using (var db = new UserContext())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
        }

        public List<User> GetUsers()
        {
            using (var db = new UserContext())
            {
                return db.Users.ToList();
            }
        }

       
        public bool UserExist(string username, string Password)
        {
            Guid pass = EncoderGuid.PasswordToGuid.Get(Password);
            using (var db = new UserContext())
            {              
                var users = db.Users.Select(c => new
                {
                    login = c.Nickname,
                    pas = c.Password
                });
                foreach (var u in users)
                {
                    if (u.login == username && u.pas == pass) return true;
                }
            }
            return false;
        }

        public User GetUserByLogin(string userName)
        {
            User user;
            using (var db = new UserContext())
            {
                return user = db.Users.Where(u => u.Nickname == userName).FirstOrDefault();
            }
        }

        public List<Comment> GetUserComments(string userName)
        {
            List<Comment> comments;
            using (var db = new UserContext())
            {
                return comments = db.Comments.Where(c => c.Author == userName).ToList();
            }
        }

        public void ConfirmEmail(string nickname)
        {
            using (var db = new UserContext())
            {
                var user = db.Users.Where(u => u.Nickname == nickname).FirstOrDefault();
                user.ConfirmedEmail = true;
                db.SaveChanges();
            }
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