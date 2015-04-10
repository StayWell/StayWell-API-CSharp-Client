using System.Collections.Generic;
using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.Content.Objects
{
    [XmlType("Content")]
    public class SegmentedContentRequest : ContentBase
    {
        [XmlArrayItem("Segment")]
        public List<ContentSegmentRequest> Segments { get; set; }

        public string Title { get; set; }
        public string InvertedTitle { get; set; }

        [XmlArrayItem("AlternateTitle")]
        public List<string> AlternateTitles { get; set; }

        public string Blurb { get; set; }

        public string LegacyId { get; set; }

        public List<ContentTaxonomyListRequest> Taxonomies { get; set; }

        public List<ServiceLineRequest> ServiceLines { get; set; } // Items of type Audience

        public List<CustomAttribute> CustomAttributes { get; set; }

        public bool Publish { get; set; }
    }
}
