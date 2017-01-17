using System.Collections.Generic;
using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.Content.Objects
{
    public class StreamingMediaResponse : SegmentedContentMetadataResponse
    {
        public string ImageUri { get; set; }
        public string ImageBucketSlug { get; set; }
        public string ImageSlug { get; set; }

        public string Transcript { get; set; }
        public string ClosedCaptioning { get; set; }
        [XmlArrayItem("Format")]
        public List<StreamingMediaFormatResponse> Formats { get; set; }
	}
}
