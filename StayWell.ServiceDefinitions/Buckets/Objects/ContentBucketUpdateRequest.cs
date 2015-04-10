using System.Collections.Generic;
using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.Buckets.Objects
{
    [XmlRoot("Bucket")]
    public class ContentBucketUpdateRequest : ContentBucketBase
    {
		[XmlArrayItem("Segment")]
		public List<ContentBucketSegmentRequest> Segments { get; set; }
	}
}


