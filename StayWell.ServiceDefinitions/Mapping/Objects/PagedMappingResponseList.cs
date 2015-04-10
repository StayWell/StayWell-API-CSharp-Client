using System.Collections.Generic;
using System.Xml.Serialization;
using StayWell.Interface;

namespace StayWell.ServiceDefinitions.Mapping.Objects
{
	[XmlType("Mappings")]
	public class PagedMappingResponseList : PagedResultList<MappingResponse>
	{
        [XmlArrayItem("StartsWith")]
        public Dictionary<string, int> StartsWith { get; set; }
	}
}
