using StayWell.ServiceDefinitions.Content.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StayWell.WebExample.Models
{
    public class ContentReferenceModel
    {
        public string Type { get; set; }
        public string BucketSlug { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string ImageUri { get; set; }
        public string Duration { get; set; }
        public long ViewCount { get; set; }
        public string Uri { get; set; }
        public string Blurb { get; set; }

        public decimal Rating { get; set; }
        public long RatingsCount { get; set; }
        public string LegacyId { get; set; }
    }
}