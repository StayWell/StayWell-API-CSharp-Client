using StayWell.ServiceDefinitions.Content.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StayWell.WebExample.Models
{
    public class LegacyContentValidationReportModel
    {
        public int ItemCount { get; set; }
        public int ErrorCount { get; set; }

        public List<LegacyContentModel> Items = new List<LegacyContentModel>();
    }
}