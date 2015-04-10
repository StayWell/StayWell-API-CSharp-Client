using System;

namespace StayWell.ServiceDefinitions.Buckets.Objects
{
	[Obsolete]
	public enum ObjectStatus
	{
		None = 0,
		Active = 1,
		Deleted = 2,
		Uploading = 3 // added for image uploading process
	}
}
