using StayWell.Interface;

namespace StayWell.ServiceDefinitions.Mapping.Objects
{
    public class MappingSearchRequest : PagedSearchRequest
	{
		public string StartsWith { get; set; }
		public bool IncludeUnpublished { get; set; }
	}
}
