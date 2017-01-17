using System;
using StayWell.ServiceDefinitions.Buckets.Objects;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	public class RelatedContentResponse
	{
		public Guid Id { get; set; }
		public string Slug { get; set; }
		public ContentBucketReference Bucket { get; set; }
		public string Title { get; set; }
		public Language Language { get; set; }

		//
		public bool IsBlocked { get; set; }

		public bool IsCustom { get; set; }
		public bool IsClientOwned { get; set; }
		public ContentType Type { get; set; }

		public int Rank { get; set; }

		// make nullable for non-streaming media? article content does not yet support views/ratings
		public long ViewCount { get; set; }
		public string LegacyId { get; set; }

		// Images/Binaries/ClientPages cannot yet be assigned mesh codes
		//public string Uri { get; set; }
	}
}
