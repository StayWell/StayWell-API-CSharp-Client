using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.Content.Objects
{
    public class MediaProviderRepositoryItemsResponse
    {
        public string Identifier { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public DateTime? DateCreated { get; set; }

        public string Error { get; set; }
    }
}
