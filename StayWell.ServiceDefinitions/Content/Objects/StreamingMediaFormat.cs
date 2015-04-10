namespace StayWell.ServiceDefinitions.Content.Objects
{
    public class StreamingMediaFormat
	{
		public string Filename { get; set; }
		public string MediaProviderSlug { get; set; }
		public string MimeType { get; set; }
		public int? VideoBitrate { get; set; }
		public int? AudioBitrate { get; set; }
		public int? Width { get; set; }
		public int? Height { get; set; }
		public int RunningTime { get; set; }
	}
}
