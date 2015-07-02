using StayWell.ServiceDefinitions.Content.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StayWell.WebExample.Models
{
    public class LegacyContentModel
    {
        public string ContentTypeId { get; set; }
        public string ContentId { get; set; }
        public bool IsAvailable { get; set; }
        public string NotAvailableReason { get; set; }
    }
}