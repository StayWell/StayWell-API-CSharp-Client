using System.Collections.Generic;
using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.Content.Objects
{
    public class ProviderStreamingMediaFormatResponse
    {
        public string AssetId { get; set; }

        public string Codec { get; set; }

        public string Container { get; set; }

        public int Duration { get; set; }

        public int EncodingRate { get; set; }

        public decimal Height { get; set; }

        public bool Remote { get; set; }

        public long Size { get; set; }

        public string Src { get; set; }

        public string StreamName { get; set; }

        public string Type { get; set; }

        public string UploadedAt { get; set; }

        public decimal Width { get; set; }
	}
}
