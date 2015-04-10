using System;
using System.Xml.Serialization;
using StayWell.Interface;

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
    }
}
