using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.Taxonomies.Objects
{
    [XmlType("TaxonomyType")]
    public class TaxonomyTypeResponse
    {
        public string Name { get; set; }
        public string Slug { get; set; }
    }
}
