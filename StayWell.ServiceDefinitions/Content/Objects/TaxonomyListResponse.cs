using System.Xml.Serialization;
using StayWell.Interface;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	[XmlRoot("Taxonomies")]
	public class TaxonomyListResponse : ResultList<ContentTaxonomyList>
	{
	}
}
