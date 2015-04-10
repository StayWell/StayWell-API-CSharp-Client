using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.Content.Objects
{
    [XmlType("ServiceLine")]
    public class ServiceLineRequest
    {
		public string AudienceSlug { get; set; }
		public string ServiceLineSlug { get; set; }
		public string PageKeywordSlug { get; set; }
    }
}
