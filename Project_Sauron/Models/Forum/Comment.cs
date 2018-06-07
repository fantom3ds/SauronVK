using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_Sauron.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public int TopicId { get; set; }
        public DateTime Pubdate { get; set; }
    }
}