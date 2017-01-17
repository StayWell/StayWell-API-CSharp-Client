using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using StayWell.Interface;
using StayWell.ServiceDefinitions.Content.Objects;

namespace StayWell.ServiceDefinitions.Collections.Objects
{
	[XmlRoot("Collection")]
	public class CollectionRequest : ResultList<CollectionItemRequest>
	{
		public string Title { get; set; }
		public DateTime? Expires { get; set; }
		public string ImageUri { get; set; }
		public string Description { get; set; }
		public string CollectionLegacyId { get; set; }
		public string RootLegacyId { get; set; }
		public string DynamicBucketIdOrSlug { get; set; }
		public string DynamicLanguageCode { get; set; }
		public List<string> DynamicMeshCodes { get; set; }
        public List<ServiceLineRequest> DynamicServiceLines { get; set; }
	}
}
