using Project_Sauron.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_Sauron.Logic
{
        public interface IForumDao
        {
            List<Topic> GetForIndex();
            Topic GetTopic(int id);
            List<Comment> GetComments(int id);
            List<Section> GetSections();
            List<Topic> GetTopicsShort(int id);
            List<Topic> GetAllTopics(int id);
            void AddComment(Comment comment);
            void AddTopic(Topic topic);
            User GetUser(string userName);
            void UpdateMessages(string userName);
        }   
}