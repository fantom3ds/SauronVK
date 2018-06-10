using Project_Sauron.Logic;
using Project_Sauron.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_Sauron.Controllers
{
    public class ForumController : Controller
    {
        private static int _topicId;
        private static int _sectionId;
        private static string _sectionName;
        ForumLogic flogic = new ForumLogic();


        public ActionResult Index()
        {
            return View(flogic.GetSections());
        }

        public ActionResult Section(int id, string sectionName)
        {
            Section section = new Section
            {
                Id = id,
                SectionName = sectionName
            };
            return View(section);
        }

        public ActionResult SectionTopicsShort(int id)
        {
            return PartialView("_SectionTopicShortPartial", flogic.GetTopicsShort(id));
        }

        public ActionResult SectionTopicsAll(int id)
        {
            return PartialView("_AllTopicsPartial", flogic.GetAllTopics(id));
        }

        public ActionResult Topic(int id)
        {
            return View(flogic.GetTopic(id));
        }

        public ActionResult Comment(int id)
        {
            return PartialView("_CommentPartial", flogic.GetComments(id));
        }

        [HttpGet]
        public ActionResult AddComment(int id)
        {
            _topicId = id;
            return PartialView("_AddCommentPartial");
        }

        [HttpPost]
        public ActionResult AddComment(CommentModel model)
        {
            if (ModelState.IsValid)
            {
                flogic.AddComment(_topicId, User.Identity.Name, model.Text);
                return RedirectToAction("Topic", "Forum", new { id = _topicId });
            }

            return PartialView("_AddCommentPartial", model);
        }

        [HttpGet]
        public ActionResult AddTopic(int id, string name)
        {
            _sectionName = name;
            _sectionId = id;
            return View();
        }

        [HttpPost]
        public ActionResult AddTopic(TopicModel model)
        {
            if (ModelState.IsValid)
            {
                flogic.AddTopic(_sectionId, User.Identity.Name, model.TopicName, model.Text);
                return RedirectToAction("Section", "Forum", new { id = _sectionId, sectionName = _sectionName });
            }

            return View(model);
        }

        public ActionResult UserInfo(string userName)
        {
            ViewBag.Time = flogic.GetUserTime(userName);
            return PartialView("_UserInfoPartial", flogic.GetUser(userName));
        }
    }
}