using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.Mapping.Objects
{
	[XmlType("Published")]
	public class PublishedChangeResponse
	{
		public int PreviouslyPublished { get; set; }
		public int NewlyPublished { get; set; }
		public int Total { get; set; }
	}
}
