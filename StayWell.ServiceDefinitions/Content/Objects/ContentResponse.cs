using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using StayWell.ServiceDefinitions.Buckets.Objects;
using StayWell.ServiceDefinitions.Common.Objects;

namespace StayWell.ServiceDefinitions.Content.Objects
{
    // TODO: Should be ContentSearchResponse

    [XmlType("Content")]
	public class ContentResponse
	{
        public Guid Id { get; set; }
		public DateTime DateAdded { get; set; }
		public DateTime DateModified { get; set; }
		public DateTime DateUpdated { get; set; }
		public string Title { get; set; }
		public string Slug { get; set; }
		public string Blurb { get; set; }
		public ContentType Type { get; set; }
        public bool IsBlocked { get; set; }
		public string Copyright { get; set; }
		public ContentBucketReference Bucket { get; set; }
		public string OriginName { get; set; }
		public string InvertedTitle { get; set; }
		[XmlArrayItem("AlternateTitle")]
		public List<string> AlternateTitles { get; set; }
		public Language Language { get; set; }
		public bool? Master { get; set; }
		public Guid? MasterId { get; set; }
		public string MasterSlug { get; set; }
		public List<ContentVersionItem> Versions { get; set; }
		[XmlArrayItem("AgeCategory")]
		public List<AgeCategory> AgeCategories { get; set; }
		public Gender? Gender { get; set; }
		public string GunningFogReadingLevel { get; set; }
		public string FleschKincaidReadingLevel { get; set; }
		[XmlArrayItem("OnlineOriginatingSource")]
		public List<OnlineOriginatingSource> OnlineOriginatingSources { get; set; }
		[XmlArrayItem("PrintOriginatingSource")]
		public List<PrintOriginatingSource> PrintOriginatingSources { get; set; }
		[XmlArrayItem("RecommendedSite")]
		public List<RecommendedSite> RecommendedSites { get; set; }
		[XmlArrayItem("OnlineEditor")]
		public List<string> OnlineEditors { get; set; }
		[XmlArrayItem("OnlineMedicalReviewer")]
		public List<string> OnlineMedicalReviewers { get; set; }
		public DateTime? LastReviewedDate { get; set; }
		public DateTime? PostingDate { get; set; }
		public DateTime? DatePublished { get; set; }
		public string LegacyId { get; set; }
		[XmlArrayItem("Author")]
		public List<string> Authors { get; set; }
		public Guid? CopyrightId { get; set; }
		public bool Published { get; set; }
		[XmlArrayItem("Taxonomy")]
		public List<ContentTaxonomyList> Taxonomies { get; set; }
		public List<ServiceLineResponse> ServiceLines { get; set; }
		public List<CustomAttribute> CustomAttributes { get; set; }
        public bool IsCustom { get; set; }
        public bool IsClientOwned { get; set; }
        public Guid? LicenseId { get; set; }

        // image:
        public string Uri { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public FormatType? Format { get; set; }
        public string AlternateText { get; set; }
        public string Caption { get; set; }
        public bool? ForFamily { get; set; }
        public Ethnicity? Ethnicity { get; set; } 
        public List<Language> AssociatedLanguages { get; set; }
        //[XmlArrayItem("Tag")]
        //public List<string> Tags { get; set; } // never assigned a value

        // streaming media:
        public string ImageUri { get; set; }
        public string Transcript { get; set; }
        public string ClosedCaptioning { get; set; }

        [XmlArrayItem("Format")]
        public List<StreamingMediaFormat> StreamingMediaFormats { get; set; }

		[XmlArrayItem("Keyword")]
		public List<string> Keywords { get; set; }
	}
}
