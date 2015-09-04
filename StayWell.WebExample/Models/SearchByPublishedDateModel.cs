using StayWell.ServiceDefinitions.Content.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StayWell.WebExample.Models
{
    public class SearchByPublishedDateModel
    {
        public DateTime FromDate { get; set; }
        public List<ContentResponse> Items { get; set; }
    }
}