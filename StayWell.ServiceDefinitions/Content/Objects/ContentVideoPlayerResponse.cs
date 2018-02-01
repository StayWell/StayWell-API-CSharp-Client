using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using StayWell.ServiceDefinitions.Buckets.Objects;
using StayWell.ServiceDefinitions.Common.Objects;

namespace StayWell.ServiceDefinitions.Content.Objects
{
    [XmlType("VideoPlayer")]
    public class ContentVideoPlayerResponse
    {
        public ContentVideoPlayerResponse()
        {
            Plugins = new List<VideoPlayerPlugin>();
        }

        public Guid Id { get; set; }

        public string VideoPlayerId { get; set; }

        public List<VideoPlayerPlugin> Plugins { get; set; }

        /// <summary>
        /// Does the player exists in Brightcove
        /// </summary>
        public bool Exists { get; set; }

        /// <summary>
        /// True when Brightcove "inactive" is false
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// If the player has unpublished changes
        /// </summary>
        public bool Modified { get; set; }

        public List<string> RelatedLicenses { get; set; }
    }

    [XmlType("Plugin")]
    public class VideoPlayerPlugin
    {
        public string Name { get; set; }

        public object Options { get; set; }
    }
}
