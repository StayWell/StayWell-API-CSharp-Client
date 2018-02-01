using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using StayWell.ServiceDefinitions.Content.Objects;

namespace StayWell.ServiceDefinitions.Content.Interfaces
{
	public interface IContentRequest : ILicensedRequest
	{
		DateTime? LastReviewedDate { get; set; }
		DateTime? PostingDate { get; set; }
		List<ContentTaxonomyListRequest> Taxonomies { get; set; }
		List<ServiceLineRequest> ServiceLines { get; set; } // Items of type Audience
		//[XmlArrayItem("Keyword")]
		List<string> Keywords { get; set; }
		bool Publish { get; set; }
		string Title { get; set; }
		string InvertedTitle { get; set; }
		string NativeLanguageTitle { get; set; }
		[XmlArrayItem("AlternateTitle")]
		List<string> AlternateTitles { get; set; }
		string Blurb { get; set; }
		string LegacyId { get; set; }
		List<CustomAttribute> CustomAttributes { get; set; }

		int? AuditUserId { get; set; }
	}
}
