using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StayWell.WebExample.Models
{
    public class GroupedContentModel
    {
        public GroupedContentModel()
        {
            Items = new List<ContentReferenceModel>();
        }
        public string Title { get; set; }
        public string Slug { get; set; }
        public List<ContentReferenceModel> Items { get; set; }
        public int Total { get; set; }
        public bool IsMoreItems { get; set; }
    }
}