
using StayWell.Interface;
using StayWell.ServiceDefinitions.Common.Objects;

namespace StayWell.ServiceDefinitions.Collections.Objects
{
	public class CollectionItemRequest : ResultList<CollectionItemRequest>
	{
		public CollectionItemType Type { get; set; }
		public ItemReference ItemReference { get; set; }
        public string Title { get; set; }
		public string Description { get; set; }
		public string LegacyId { get; set; }
		public bool Flagged { get; set; }
        public string FlagComment { get; set; }
        public bool? Disabled { get; set; }
		public string ImageUri { get; set; }
	}
}
