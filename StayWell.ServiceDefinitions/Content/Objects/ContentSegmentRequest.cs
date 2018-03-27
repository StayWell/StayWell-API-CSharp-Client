using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	public class ContentSegmentRequest
	{
		public string IdOrSlug { get; set; }
		public string CustomName { get; set; }
		[XmlElement]
		public string Body { get; set; }
	}
}
