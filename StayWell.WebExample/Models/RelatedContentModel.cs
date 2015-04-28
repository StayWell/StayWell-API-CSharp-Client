using StayWell.ServiceDefinitions.Content.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StayWell.WebExample.Models
{
    public class RelatedContentModel
    {
        public string OriginalArticleTitle { get; set; }
        public List<ContentResponse> RelatedContent { get; set; }
    }
}