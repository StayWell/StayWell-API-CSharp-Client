using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.Mapping.Objects
{
	[XmlType("Mapping")]
	public class MappingRequest
	{
		public string Name { get; set; }
		public string Uri { get; set; }
		public bool Published { get; set; }
	}
}
