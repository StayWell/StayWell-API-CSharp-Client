using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.Content.Objects
{
    [XmlType("ServiceLine")]
    public class ServiceLineResponse
    {
		public string AudienceSlug { get; set; }
		public string ServiceLineSlug { get; set; }
		public string PageKeywordSlug { get; set; }

		public string Audience { get; set; }
		public string ServiceLine { get; set; }
		public string PageKeyword { get; set; }
    }
}
