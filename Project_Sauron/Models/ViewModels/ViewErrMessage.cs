using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_Sauron.Models.ViewModels
{
    public class ViewErrMessage
    {
        public string Head { get; set; }
        public string Advice { get; set; }

        public string Message { get; set; }
        public string TargetSite { get; set; }
        public string Inner { get; set; }

        public ViewErrMessage(Exception Ex, string Head="", string Advice="")
        {
            Message = Ex.Message; TargetSite = Ex.TargetSite.ToString();
            Inner = Ex.InnerException.TargetSite + ": " + Ex.InnerException.Message;
            this.Advice = Advice;
            this.Head = Head;
        }

        public ViewErrMessage( string Head = "", string Advice = "")
        {
            this.Advice = Advice;
            this.Head = Head;
        }
    }
}