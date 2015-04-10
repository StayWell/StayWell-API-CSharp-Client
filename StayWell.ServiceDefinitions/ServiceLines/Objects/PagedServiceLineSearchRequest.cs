using StayWell.Interface;

namespace StayWell.ServiceDefinitions.ServiceLines.Objects
{
    public class PagedServiceLineSearchRequest : PagedSearchRequest
	{
		public string AudienceSlug { get; set; }
	}
}
