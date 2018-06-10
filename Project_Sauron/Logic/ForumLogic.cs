using Project_Sauron.Controllers;
using Project_Sauron.DataAccesLayer;
using Project_Sauron.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_Sauron.Logic
{
    public class ForumLogic
    {
        ForumDao fdao = new ForumDao();

        public List<Topic> GetForIndex() => fdao.GetForIndex();
        public Topic GetTopic(int id) => fdao.GetTopic(id);
        public List<Comment> GetComments(int id) => fdao.GetComments(id);
        public List<Section> GetSections() => fdao.GetSections();
        public List<Topic> GetTopicsShort(int id) => fdao.GetTopicsShort(id);
        public List<Topic> GetAllTopics(int id) => fdao.GetAllTopics(id);

        public void AddComment(int topicId, string author, string text)
        {
            Comment comment = new Comment()
            {
                TopicId = topicId,
                Author = author,
                Text = text,
                Pubdate = DateTime.Now
            };
            fdao.AddComment(comment);
            fdao.UpdateMessages(author);
        }

        public void AddTopic(int sectionId, string author, string topicName, string text)
        {
            User user = fdao.GetUser(author);
            Topic topic = new Topic()
            {
                TopicName = topicName,
                SectionId = sectionId,
                Text = text,
                Author = author,
                Pubdate = DateTime.Now

            };

            fdao.AddTopic(topic);
            fdao.UpdateMessages(author);
        }

        public User GetUser(string userName)
        {
            return fdao.GetUser(userName);
        }

        public string GetUserTime(string userName)
        {
            User user = fdao.GetUser(userName);
            TimeSpan time = (DateTime.Now - user.RegDate);
            string days = time.ToString("%d");
            string hours = time.ToString("%h");
            string minutes = time.ToString("%m");
            string userTime = $"{days}д {hours}ч {minutes}м";
            return userTime;
        }

    }
}
