using System.Collections.Generic;
using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.ServiceLines.Objects
{
	[XmlRoot("ServiceLine")]
	public class ServiceLineItemResponse
	{
		public string Audience { get; set; }
		public string AudienceSlug { get; set; }
		public string ServiceLine { get; set; }
		public string ServiceLineSlug { get; set; }
		public List<KeywordResponse> Keywords { get; set; }
	}
}
