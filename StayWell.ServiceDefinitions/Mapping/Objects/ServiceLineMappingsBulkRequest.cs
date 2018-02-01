using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayWell.ServiceDefinitions.Mapping.Objects
{
    public class ServiceLineMappingsBulkRequest
    {
        public List<string> Paths { get; set; }
        public bool IncludeUnpublished { get; set; }
    }
}
