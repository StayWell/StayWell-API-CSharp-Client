using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using StayWell.ServiceDefinitions.Buckets.Objects;
using StayWell.ServiceDefinitions.Common.Objects;
using StayWell.ServiceDefinitions.Content.Interfaces;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	[XmlType("Binary")]
	public class BinaryDetailResponse : IContentMetaDataBaseResponse /*: BinaryDetailRequest */
	{
		// new from ContentBase
		public Guid? LicenseId { get; set; }
		//public DateTime? LastReviewedDate { get; set; }
		//public DateTime? PostingDate { get; set; }
		

		// new from ContentMetadataResponse
		public DateTime DateUpdated { get; set; }
		public ContentType Type { get; set; }
		public Language Language { get; set; }
		public bool Master { get; set; }
		public Guid MasterId { get; set; }
		public string MasterSlug { get; set; }
		public List<ContentVersionItem> Versions { get; set; }
		public List<ContentTaxonomyList> Taxonomies { get; set; }
		public List<ServiceLineResponse> ServiceLines { get; set; }
		[XmlArrayItem("Keyword")]
		public List<string> Keywords { get; set; }
		public bool Published { get; set; }
		public DateTime? DatePublished { get; set; }
		public bool IsCustom { get; set; }
		public bool IsClientOwned { get; set; }
		

		// original
		public Guid Id { get; set; }
		public ContentBucketReference Bucket { get; set; }
		public string Uri { get; set; }
		public FormatType Format { get; set; }
		public DateTime DateAdded { get; set; }
		public DateTime DateModified { get; set; }
        public string OriginName { get; set; }


		// from BinaryDetailRequest
		public string Title { get; set; }
		public string Slug { get; set; }
		public string InvertedTitle { get; set; }
		[XmlArrayItem("AlternateTitle")]
		public List<string> AlternateTitles { get; set; }
		public string Blurb { get; set; }
		public DateTime? PostingDate { get; set; }

		public List<CustomAttribute> CustomAttributes { get; set; }
		
		
		
		public string LegacyId { get; set; }
		

		// binary only
		[XmlArrayItem("Tag")]
		public List<string> Tags { get; set; }
		public Guid? Replacement { get; set; }

		public double Rating { get; set; }
		public long RatingsCount { get; set; }
		public long ViewCount { get; set; }

		public List<AgeCategory> AgeCategories { get; set; }
		public Gender Gender { get; set; }
	}
}
