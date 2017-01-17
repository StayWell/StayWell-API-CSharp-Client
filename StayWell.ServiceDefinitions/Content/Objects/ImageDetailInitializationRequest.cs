using StayWell.ServiceDefinitions.Buckets.Objects;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	public class ImageDetailInitializationRequest: ImageDetailRequest
	{
		// key off this class for setting slug and status
		public FileStatus Status { get; set; }
	}
}
