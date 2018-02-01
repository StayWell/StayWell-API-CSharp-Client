using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using StayWell.ServiceDefinitions.Buckets.Objects;
using StayWell.ServiceDefinitions.Common.Objects;

namespace StayWell.ServiceDefinitions.Content.Objects
{
    [XmlType("VideoPlayer")]
    public class ContentVideoPlayerRequest
    {
        public string VideoPlayerId { get; set; }

        public List<VideoPlayerPlugin> Plugins { get; set; }

        public bool Active { get; set; }
    }
}
