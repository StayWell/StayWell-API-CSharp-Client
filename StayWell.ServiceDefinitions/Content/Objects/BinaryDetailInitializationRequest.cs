using StayWell.ServiceDefinitions.Buckets.Objects;
using StayWell.ServiceDefinitions.Content.Interfaces;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	public class BinaryDetailInitializationRequest : BinaryDetailRequest, IContentRequest
	{
		// key off this class for setting slug and status
		public FileStatus Status { get; set; }
	}
}
