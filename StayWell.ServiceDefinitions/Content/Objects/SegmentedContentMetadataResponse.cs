using System.Collections.Generic;
using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.Content.Objects
{
    [XmlType("Content")]
    public class SegmentedContentMetadataResponse : ContentMetadataResponse
    {
        [XmlArrayItem("Segment")]
        public List<ContentSegmentResponse> Segments { get; set; }
        public string Copyright { get; set; }
    }
}
