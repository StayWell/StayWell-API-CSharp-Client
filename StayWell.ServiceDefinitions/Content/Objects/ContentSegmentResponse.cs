using System;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	public class ContentSegmentResponse
	{
        public Guid? Id { get; set; }
        public string Slug { get; set; }
		public string Name { get; set; }
		public string CustomName { get; set; }
		public string Body { get; set; }
	}
}
