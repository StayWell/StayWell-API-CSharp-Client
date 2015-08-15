using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StayWell.WebExample.Models
{
    public class ShowServiceLinesModel
    {
        public ShowServiceLinesModel()
        {
            Audiences = new List<string>();
            ServiceLines = new List<string>();
            PageKeywords = new List<string>();
            InvalidServiceLines = new List<string>();
        }
        public List<string> Audiences { get; set; }
        public List<string> ServiceLines { get; set; }
        public List<string> PageKeywords { get; set; }

        public List<string> InvalidServiceLines { get; set; }

        public string audienceSlug { get; set; }
        public string serviceLineSlug { get; set; }


    }
}