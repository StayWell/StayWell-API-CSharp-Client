using System.Collections.Generic;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	public class RelatedContentSearchRequest
	{
		public int ItemsPerBucket { get; set; }
		public List<string> MeshCodes { get; set; } // if used, ignore bucket/content
		public string BucketIdOrSlug { get; set; } // if used and language not specified, use content language
		public string ContentIdOrSlug { get; set; }

		public bool IncludeBlockedContent { get; set; }
		public bool GetOriginal { get; set; }
		public string Language { get; set; }

		public int? MaxBuckets { get; set; }

		public List<ContentType> ContentTypes { get; set; }

		// currently, there is no way to assign mesh codes to client pages
		//public bool ExcludeClientPages { get; set; }
	}
}
