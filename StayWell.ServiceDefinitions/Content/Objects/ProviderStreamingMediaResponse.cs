using System.Collections.Generic;
using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.Content.Objects
{
    public class ProviderStreamingMediaResponse
    {
        public string Id { get; set; }

        public string AccountId { get; set; }

        public string LegacyId { get; set; }

        public string Description { get; set; }

        public int Duration { get; set; }

        public string Name { get; set; }

        public string OriginalFilename { get; set; }

        public string PublishedAt { get; set; }

        public string ReferenceId { get; set; }

        public string State { get; set; }

        public string UpdatedAt { get; set; }
	}
}
