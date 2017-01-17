using System;
using System.Collections.Generic;
using StayWell.ServiceDefinitions.Buckets.Objects;
using StayWell.ServiceDefinitions.Common.Objects;
using StayWell.ServiceDefinitions.Content.Objects;

namespace StayWell.ServiceDefinitions.Content.Interfaces
{
	public interface IContentMetaDataBaseResponse
	{
		Guid Id { get; set; }
		ContentBucketReference Bucket { get; set; }
		string OriginName { get; set; }
		Guid? LicenseId { get; set; }
		DateTime DateAdded { get; set; }
		DateTime DateModified { get; set; }
		DateTime DateUpdated { get; set; }
		ContentType Type { get; set; }
		bool Master { get; set; }
		Guid MasterId { get; set; }
		string MasterSlug { get; set; }
		List<ContentVersionItem> Versions { get; set; }
		List<ContentTaxonomyList> Taxonomies { get; set; }
		List<ServiceLineResponse> ServiceLines { get; set; }
		//[XmlArrayItem("Keyword")]
		List<string> Keywords { get; set; }
		bool Published { get; set; }
		DateTime? DatePublished { get; set; }
		bool IsCustom { get; set; }
		bool IsClientOwned { get; set; }
		string Title { get; set; }
		string Slug { get; set; }
		string InvertedTitle { get; set; }
		//[XmlArrayItem("AlternateTitle")]
		List<string> AlternateTitles { get; set; }
		string Blurb { get; set; }
		DateTime? PostingDate { get; set; }
		//Guid? Replacement { get; set; }
		string LegacyId { get; set; }
		

		//[XmlArrayItem("Tag")]
		//List<string> Tags { get; set; }
		
		List<CustomAttribute> CustomAttributes { get; set; }
		Language Language { get; set; }

		double Rating { get; set; }
		long RatingsCount { get; set; }
		long ViewCount { get; set; }


		List<AgeCategory> AgeCategories { get; set; }
		Gender Gender { get; set; }
	}
}