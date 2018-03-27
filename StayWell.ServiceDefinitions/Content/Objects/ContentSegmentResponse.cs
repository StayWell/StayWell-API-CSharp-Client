using System;
using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	public class ContentSegmentResponse
	{
        public Guid? Id { get; set; }
        public string Slug { get; set; }
		public string Name { get; set; }
		public string CustomName { get; set; }
		[XmlElement]
		public string Body { get; set; }
	}
}
