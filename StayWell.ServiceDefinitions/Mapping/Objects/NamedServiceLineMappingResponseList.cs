using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayWell.ServiceDefinitions.Mapping.Objects
{
    public class NamedServiceLineMappingResponseList : ServiceLineMappingResponseList
    {
        public string ServiceLine { get; set; }
    }
}
