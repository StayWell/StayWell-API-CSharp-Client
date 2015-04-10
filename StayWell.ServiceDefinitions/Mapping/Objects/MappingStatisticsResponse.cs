using System.Xml.Serialization;
using StayWell.Interface;

namespace StayWell.ServiceDefinitions.Mapping.Objects
{
	[XmlType("MappingStatistics")]
	public class MappingStatisticsResponse : ResultList<MappingCountResponse>
	{
	}
}
