using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using StayWell.Interface;
using StayWell.ServiceDefinitions.Common.Objects;
using StayWell.ServiceDefinitions.Content.Objects;

namespace StayWell.ServiceDefinitions.Collections.Objects
{
	[XmlRoot("Collection")]
	public class CollectionResponse : ResultList<CollectionItemResponse>
    {
		[Document("A unique identifier for the collection")]
        public Guid Id { get; set; }

		[Document("The title of the collection")]
        public string Title { get; set; }

		[Document("A human-readable unique identifier")]
        public string Slug { get; set; }

		[Document("The date and time that the collection was created")]
        public DateTime? DateAdded { get; set; }

		[Document("The date and time that the collection was last modified")]
        public DateTime? DateModified { get; set; }

		[Document("The user that created the collection")]
        public UserDetailsResponse CreatedBy { get; set; }

		[Document("An optional date after which the collection will no longer be available")]
        public DateTime? Expires { get; set; }

		[Document("The URL for an image that relates to the collection")]
        public string ImageUri { get; set; }

        // TODO: Add these?
        /*
        public string ImageBucketSlug { get; set; }
        public string ImageSlug { get; set; }
        */

		[Document("A description of the collection")]
        public string Description { get; set; }

		[Document("A unique identifier that is used by a legacy system for the root topic")]
        public string RootLegacyId { get; set; }

		[Document("A unique identifier that is used by a legacy system for the collection")]
		public string CollectionLegacyId { get; set; }

		[Document("The number of licenses that use the collection")]
		public int LicenseCount { get; set; }

        public List<ContentType> ContentTypes { get; set; }
    }
}
