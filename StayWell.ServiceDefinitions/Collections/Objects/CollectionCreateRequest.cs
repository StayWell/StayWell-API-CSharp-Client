using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.Collections.Objects
{
    [XmlRoot("Collection")]
    public class CollectionCreateRequest : CollectionRequest
    {
        public string Slug { get; set; }
    }
}
