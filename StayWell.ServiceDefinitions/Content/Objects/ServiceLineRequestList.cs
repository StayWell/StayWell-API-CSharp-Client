using System.Xml.Serialization;
using StayWell.Interface;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	[XmlRoot("ServiceLines")]
	public class ServiceLineRequestList : ResultList<ServiceLineRequest>
	{
	}
}
