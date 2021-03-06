﻿using Project_Sauron.Models;
using System.Collections.Generic;
using System.Linq;

namespace Project_Sauron.DataAccesLayer
{
    public class ForumDao
    {
        public List<Topic> GetForIndex()
        {
            List<Topic> topics;
            using (var db = new UserContext())
            {
                return topics = db.Topics.OrderByDescending(t => t.Pubdate).Take(5).ToList();
            }
        }

        public Topic GetTopic(int id)
        {
            Topic topic;
            using (var db = new UserContext())
            {
                return topic = db.Topics.Find(id);
            }
        }

        public List<Comment> GetComments(int id)
        {
            List<Comment> comments;
            using (var db = new UserContext())
            {
                return comments = db.Comments.Where(c => c.TopicId == id).ToList();
            }
        }

        public List<Section> GetSections()
        {
            List<Section> sections;
            using (var db = new UserContext())
            {
                return sections = db.Sections.ToList();
            }
        }

        public List<Topic> GetTopicsShort(int id)
        {
            List<Topic> topics;
            using (var db = new UserContext())
            {
                return topics = db.Topics.Where(t => t.SectionId == id).Take(3).ToList();
            }
        }

        public List<Topic> GetAllTopics(int id)
        {
            List<Topic> topics;
            using (var db = new UserContext())
            {
                return topics = db.Topics.Where(t => t.SectionId == id).ToList();
            }
        }

        public void AddComment(Comment comment)
        {
            using (var db = new UserContext())
            {
                db.Comments.Add(comment);
                db.SaveChanges();
            }
        }

        public void AddTopic(Topic topic)
        {
            using (var db = new UserContext())
            {
                db.Topics.Add(topic);
                db.SaveChanges();
            }
        }

        public User GetUser(string nickname)
        {
            User user = null;
            using (var db = new UserContext())
            {
                return user = db.Users.FirstOrDefault(u => u.Login == nickname);
            }
        }

        public void UpdateMessages(string nickname)
        {
            User user;
            using (var db = new UserContext())
            {
                user = db.Users.FirstOrDefault();
                user.Messages += 1;
                db.SaveChanges();
            }
        }
    }
}
