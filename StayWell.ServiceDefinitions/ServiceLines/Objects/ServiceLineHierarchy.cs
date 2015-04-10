using System.Xml.Serialization;
using StayWell.Interface;

namespace StayWell.ServiceDefinitions.ServiceLines.Objects
{
	[XmlRoot("ServiceLineHierchy")]
	public class ServiceLineHierarchy : ResultList<ServiceLineHierarchyItem>
	{
	}
}
