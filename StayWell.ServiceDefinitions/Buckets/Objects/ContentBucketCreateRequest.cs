using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.Buckets.Objects
{
    [XmlRoot("Bucket")]
    public class ContentBucketCreateRequest : ContentBucketUpdateRequest
    {
        public string Slug { get; set; }
    }
}
