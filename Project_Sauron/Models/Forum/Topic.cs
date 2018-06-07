using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_Sauron.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string TopicName { get; set; }
        public int SectionId { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public string Email { get; set; }
        public DateTime Pubdate { get; set; }
    }
}