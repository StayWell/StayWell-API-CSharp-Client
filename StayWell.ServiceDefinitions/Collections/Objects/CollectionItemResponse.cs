using System;
using System.Collections.Generic;
using StayWell.Interface;
using StayWell.ServiceDefinitions.Buckets.Objects;
using StayWell.ServiceDefinitions.Content.Objects;

namespace StayWell.ServiceDefinitions.Collections.Objects
{
	public class CollectionItemResponse : ResultList<CollectionItemResponse>
	{
		public CollectionItemType Type { get; set; }

		public Guid? CollectionStructureId { get; set; }
		public Guid Id { get; set; }
		public string Slug { get; set; }
		public string LegacyId { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public bool? Flagged { get; set; }
		public string FlagComment { get; set; }
		public bool? Disabled { get; set; }
		public DateTime DateAdded { get; set; }

		// topic only:
		public List<ContentType> ContentTypes { get; set; }
		public string ImageUri { get; set; }
		// TODO: Add these? (in line with images in streaming media)
		/*
		public string ImageBucketSlug { get; set; }
		public string ImageSlug { get; set; }
		*/

        // TODO: add property for BucketFilterReference for clarity (content vs dynamic topic)? (public ContentBucketReference BucketId)
        public ContentBucketReference Bucket { get; set; }

        // TODO: Rename to reflect they are for dynamic topic
        public Language Language { get; set; }
		public List<string> MeshCodes { get; set; }
        
		// content only (convenience info):
        public ContentType ContentType { get; set; }
		public List<ServiceLineResponse> ServiceLines { get; set; }
		public ContainerType ContainerType { get; set; }
        //public ContentBucketReference Bucket { get; set; }
	}
}
