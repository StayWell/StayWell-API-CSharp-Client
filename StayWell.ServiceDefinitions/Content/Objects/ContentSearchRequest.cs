using System;
using System.Collections.Generic;
using StayWell.Interface;

namespace StayWell.ServiceDefinitions.Content.Objects
{
    public class ContentSearchRequest : PagedSearchRequest
	{
		public List<ContentSortBy> SortBy { get; set; }

		public string TitleStartsWith { get; set; }

		// this specifies whther to include alternate titles in the title-starts-with filter
		public bool IncludeAlternateTitles { get; set; }

		public bool IncludeBlockedContent { get; set; }

        // bucket Ids and slugs
		public List<string> Buckets { get; set; }

		public List<string> Collections { get; set; }
		public List<string> Languages { get; set; }
		public List<SearchType> Types { get; set; }
		public List<string> LegacyIds { get; set; }
		public List<FormatType> Formats { get; set; }
		public bool IncludeDrafts { get; set; }
        public List<Guid> Licenses { get; set; }
        public List<Guid> Clients { get; set; }
        public SearchOption? Option { get; set; }
        public List<SearchExclusion> Exclude { get; set; }
		public List<string> ServiceLines { get; set; }

        // assumes Types includes Content
        public List<ContentType> ContentTypes { get; set; }

		public bool ExcludeClientPages { get; set; }

		// taxonomies (OR'd by values and type)
		public List<string> Icd9 { get; set; }
		public List<string> Cpt { get; set; }
		public List<string> Hcpcs { get; set; }
		public List<string> Icd10Cm { get; set; }
		public List<string> Icd10Pcs { get; set; }
		public List<string> Loinc { get; set; }
		public List<string> Mesh { get; set; }
		public List<string> Ndc { get; set; }
		public List<string> RxNorm { get; set; }
		public List<string> SnoMed { get; set; }
	}
}
