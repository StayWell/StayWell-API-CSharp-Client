using System.Xml.Serialization;
using StayWell.Interface;

namespace StayWell.ServiceDefinitions.Mapping.Objects
{
	[XmlRoot("ServiceLineReport")]
	public class MappingServiceLineReport : PagedResultList<MappingServiceLineReportItem>
	{

	}
}
