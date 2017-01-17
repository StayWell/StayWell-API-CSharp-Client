using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using StayWell.ServiceDefinitions.Buckets.Objects;
using StayWell.ServiceDefinitions.Content.Interfaces;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	[XmlType("Content")]
	public class ContentMetadataResponse : ContentBase, IContentMetaDataBaseResponse
	{
		public Guid Id { get; set; }

		public ContentBucketReference Bucket { get; set; }

		public string OriginName { get; set; }

		public DateTime DateAdded { get; set; }

		public DateTime DateModified { get; set; }

		public DateTime DateUpdated { get; set; }

		public ContentType Type { get; set; }

		public string Title { get; set; }

		public string InvertedTitle { get; set; }

		[XmlArrayItem("AlternateTitle")]
		public List<string> AlternateTitles { get; set; }

		public string Slug { get; set; }

		public string Blurb { get; set; }

		public Language Language { get; set; }

		public bool Master { get; set; }

		public Guid MasterId { get; set; }

		public string MasterSlug { get; set; }

		public List<ContentVersionItem> Versions { get; set; }

		public List<ContentTaxonomyList> Taxonomies { get; set; }

		public List<ServiceLineResponse> ServiceLines { get; set; }

		public List<CustomAttribute> CustomAttributes { get; set; }

		[XmlArrayItem("Keyword")]
		public List<string> Keywords { get; set; }

		public string LegacyId { get; set; }

		public bool Published { get; set; }

		public DateTime? DatePublished { get; set; }
		
        public bool IsCustom { get; set; }

        public bool IsClientOwned { get; set; }

		public double Rating { get; set; }
		public long RatingsCount { get; set; }
		public long ViewCount { get; set; }
	}
}
