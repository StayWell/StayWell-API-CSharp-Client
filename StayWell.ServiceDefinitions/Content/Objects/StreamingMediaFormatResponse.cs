namespace StayWell.ServiceDefinitions.Content.Objects
{
    public class StreamingMediaFormatResponse
    {
        public string Filename { get; set; }
        public string MediaProviderSlug { get; set; }
		public string MediaProviderName { get; set; }
        public string Uri { get; set; }
        public string MimeType { get; set; }
		public string FormatName { get; set; }
        public FileFormatMediaType MediaType { get; set; }
        public int? VideoBitrate { get; set; }
        public int? AudioBitrate { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int RunningTime { get; set; }
    }
}
