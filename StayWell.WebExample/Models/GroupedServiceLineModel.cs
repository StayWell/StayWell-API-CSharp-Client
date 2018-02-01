using StayWell.ServiceDefinitions.Mapping.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StayWell.WebExample.Models
{
    public class GroupedRelatedServices
    {
        public GroupedRelatedServices()
        {
            RelatedServices = new List<MappingResponse>();
        }
        public string Group { get; set; }
        public List<MappingResponse> RelatedServices { get; set; }
    }
}