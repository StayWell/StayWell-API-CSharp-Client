using StayWell.ServiceDefinitions.Content.Objects;
using StayWell.ServiceDefinitions.Mapping.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StayWell.WebExample.Models
{
    public class ContentWithRelationshipModel
    {
        public ContentWithRelationshipModel()
        {
            RelatedContent = new List<GroupedContentModel>();
        }

        public ContentArticleResponse Article {get;set;}
        public StreamingMediaResponse StreamingMedia { get; set; }

        public List<GroupedContentModel> RelatedContent { get; set; }
    }
}