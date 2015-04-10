using System;
using System.Collections.Generic;
using StayWell.Interface;

namespace StayWell.ServiceDefinitions.Content.Objects
{
    public class ContentSearchRequest : PagedSearchRequest
	{
		public string TitleStartsWith { get; set; }
		public bool? IncludeAlternateTitles { get; set; }
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
	}
}
