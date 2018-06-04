using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_Sauron.Models
{
    public class Error
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Message { get; set; }

        [MaxLength(50)]
        public string TargetSite { get; set; }

        public long Time { get; set; }
    }
}