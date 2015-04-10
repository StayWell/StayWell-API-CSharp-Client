using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using StayWell.ServiceDefinitions.Common.Objects;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	public class ContentBase
	{
		[XmlArrayItem("AgeCategory")]
		public List<AgeCategory> AgeCategories { get; set; }

		public Gender Gender { get; set; }

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

		[XmlArrayItem("Author")]
		public List<string> Authors { get; set; }

		public Guid? CopyrightId { get; set; }

		public Guid? LicenseId { get; set; }
	}
}
