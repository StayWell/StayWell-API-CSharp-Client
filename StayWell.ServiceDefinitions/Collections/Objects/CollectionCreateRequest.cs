using System.Xml.Serialization;
using StayWell.ServiceDefinitions.Content.Objects;

namespace StayWell.ServiceDefinitions.Collections.Objects
{
	[XmlRoot("Collection")]
	public class CollectionCreateRequest : CollectionRequest
	{
		public string Slug { get; set; }
		public ContainerType ContainerType { get; set; }
	}
}
