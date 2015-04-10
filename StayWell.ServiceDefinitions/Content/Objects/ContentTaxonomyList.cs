using System.Xml.Serialization;
using StayWell.Interface;

namespace StayWell.ServiceDefinitions.Content.Objects
{
    [XmlType("Taxonomy")]
    public class ContentTaxonomyList : ResultList<ContentTaxonomyValue>
    {
        public string Slug { get; set; }
        public string Name { get; set; }
    }
}