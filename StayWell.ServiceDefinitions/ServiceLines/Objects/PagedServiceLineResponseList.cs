using System.Xml.Serialization;
using StayWell.Interface;

namespace StayWell.ServiceDefinitions.ServiceLines.Objects
{
	[XmlRoot("ServiceLines")]
	public class PagedServiceLineResponseList : PagedResultList<ServiceLineItemResponse>
	{
	}
}
