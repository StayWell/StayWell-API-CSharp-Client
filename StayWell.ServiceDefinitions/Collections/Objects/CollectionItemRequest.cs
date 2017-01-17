
using System.Collections.Generic;
using StayWell.Interface;
using StayWell.ServiceDefinitions.Common.Objects;
using StayWell.ServiceDefinitions.Content.Objects;

namespace StayWell.ServiceDefinitions.Collections.Objects
{
    // TODO: add validation
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
		public ContainerType ContainerType { get; set; }

        // TODO: Rename to reflect they are for dynamic topic
		public string BucketIdOrSlug { get; set; }
		public string LanguageCode { get; set; }
		public List<string> MeshCodes { get; set; }
        public List<ServiceLineRequest> ServiceLines { get; set; }
	}
}
