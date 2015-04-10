using System;
using StayWell.ServiceDefinitions.Content.Objects;

namespace StayWell.ServiceDefinitions.Buckets.Objects
{
	public class ContentBucketBase
	{
		public ContentType Type { get; set; }
		public string Description { get; set; }
		public string Name { get; set; }
		public bool ReadOnly { get; set; }
		public int? LegacyId { get; set; }
		public Guid OriginId { get; set; }
		public ContentBucketConstraint Constraints { get; set; }
		public Guid? CopyrightId { get; set; }
	}
}
