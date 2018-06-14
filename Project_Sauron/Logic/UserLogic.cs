using Project_Sauron.DataAccesLayer;
using Project_Sauron.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Project_Sauron.Logic
{
    public class UserLogic
    {
        UserDao udao = new UserDao();
        public List<User> GetUsers()=>udao.GetUsers();

        //Тут сама разберись, чо да как, откуда ты берешь строку пароля
        Guid password = EncoderGuid.PasswordToGuid.Get(model.Password);

        public void AddUser(string nickname, string password, string email)
        {
            //Сложно было??
            Guid pass = EncoderGuid.PasswordToGuid.Get(password);
            User user = new User()
            {
                Nickname = nickname,
                Password = pass,
                Email = email,
                RegDate = DateTime.Now
            };

            udao.Add(user);
        }

        public bool CheckUser(string nickname, string password)
        {
            User user = udao.GetUserByLogin(nickname);
            if (udao.UserExist(nickname, password) && user.ConfirmedEmail) return true;
            return false;
        }


        public bool CheckUserReg(string nickname)
        {
            var users = udao.GetUsers();
            foreach (var u in users)
            {
                if (nickname == u.Nickname) return true;
            }
            return false;
        }

        public User GetUserByLogin(string nickname)
        {
            return udao.GetUserByLogin(nickname);
        }

        public List<Comment> GetUserComments(string nickname)
        {
            return udao.GetUserComments(nickname);
        }

        public void ConfirmEmail(string nickname)
        {
            udao.ConfirmEmail(nickname);
        }

    }
}
