using StayWell.Interface;

namespace StayWell.ServiceDefinitions.ServiceLines.Objects
{
	public class ServiceLineHierarchyItem : ResultList<ServiceLineHierarchyItem>
	{
		public string Slug { get; set; }
		public string Name { get; set; }
	}
}
