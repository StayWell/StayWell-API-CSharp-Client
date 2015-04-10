using System.Xml.Serialization;
using StayWell.Interface;

namespace StayWell.ServiceDefinitions.Content.Objects
{
    [XmlType("Taxonomy")]
	public class ContentTaxonomyListRequest : ResultList<ContentTaxonomyValue>
    {
        public string Slug { get; set; }
    }
}
