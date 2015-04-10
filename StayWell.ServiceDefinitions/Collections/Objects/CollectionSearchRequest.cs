using StayWell.Interface;

namespace StayWell.ServiceDefinitions.Collections.Objects
{
    public class CollectionSearchRequest : PagedSearchRequest
    {
		public bool? Flagged { get; set; }
        public string TitleStartsWith { get; set; }

        public CollectionSearchSortField? SortField1 { get; set; }
        public SortDirection? SortDirection1 { get; set; }

        public CollectionSearchSortField? SortField2 { get; set; }
        public SortDirection? SortDirection2 { get; set; }

        public CollectionSearchSortField? SortField3 { get; set; }
        public SortDirection? SortDirection3 { get; set; }
    }
}
