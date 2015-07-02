using StayWell.ServiceDefinitions.Content.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StayWell.WebExample.Models
{
    public class ContentModel
    {
        public string Type { get; set; }
        public string BucketSlug { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
    }
}