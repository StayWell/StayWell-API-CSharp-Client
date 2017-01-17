using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using StayWell.ServiceDefinitions.Common.Objects;
using StayWell.ServiceDefinitions.Content.Interfaces;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	[XmlType("Binary")]
	public class BinaryDetailRequest : IContentRequest
	{
		// language versions and master binaries?


		// from ContentBase
		public Guid? LicenseId { get; set; }
		public DateTime? LastReviewedDate { get; set; }
		public DateTime? PostingDate { get; set; }
		

		// from SegmentedContentRequest
		public List<ContentTaxonomyListRequest> Taxonomies { get; set; }
		public List<ServiceLineRequest> ServiceLines { get; set; } // Items of type Audience
		[XmlArrayItem("Keyword")]
		public List<string> Keywords { get; set; }
		public bool Publish { get; set; } //?


		// original
		public string Title { get; set; }
		
		public string InvertedTitle { get; set; }
		[XmlArrayItem("AlternateTitle")]
		public List<string> AlternateTitles { get; set; }
		public string Blurb { get; set; }
		public List<CustomAttribute> CustomAttributes { get; set; }
		public string LegacyId { get; set; }



		
		// this should only exist in the create request, but the create request happens when the binary is created and the slug is not available
		public string Slug { get; set; }


		public Gender Gender { get; set; }
		public List<AgeCategory> AgeCategories { get; set; }


		// binary only

		public Guid? Replacement { get; set; }
		[XmlArrayItem("Tag")]
		public List<string> Tags { get; set; }
	}
	
}
