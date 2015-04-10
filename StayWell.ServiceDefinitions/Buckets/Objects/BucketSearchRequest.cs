using System;
using System.Collections.Generic;
using StayWell.Interface;
using StayWell.ServiceDefinitions.Content.Objects;

namespace StayWell.ServiceDefinitions.Buckets.Objects
{
	public class BucketSearchRequest : PagedSearchRequest
	{
		public List<ContentType> Type { get; set; }
		public string Origin { get; set; }
		public int? LegacyId { get; set; }
		public bool? ReadOnly { get; set; }
		public List<Guid> CopyrightIds { get; set; }
		public List<Guid> Ids { get; set; }
		public List<string> Slugs { get; set; } 
		public bool? HasDefaultCopyrightId { get; set; }
		public string NameStartsWith { get; set; }
		public List<Guid> Licenses { get; set; }
		public List<Guid> Clients { get; set; }
	}
}
