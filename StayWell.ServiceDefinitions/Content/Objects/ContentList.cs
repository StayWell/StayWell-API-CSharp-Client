using System.Xml.Serialization;
using StayWell.Interface;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	[XmlType("Content")]
	public class ContentList : PagedResultList<ContentResponse>
	{
		public TypeCount TypeCounts { get; set; }
		public bool IncludeBlockedContent { get; set; }
	}
}
