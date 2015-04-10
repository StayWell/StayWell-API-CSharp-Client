using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using StayWell.ServiceDefinitions.Content.Objects;

namespace StayWell.ServiceDefinitions.Buckets.Objects
{
	[XmlRoot("Bucket")]
	public class ContentBucketResponse : ContentBucketBase
	{
		public Guid Id { get; set; }
        public string Slug { get; set; }
		public ObjectStatus Status { get; set; }
		public string OriginName { get; set; }
		public ContentOriginType OriginType { get; set; }
		public string TypeDescription { get; set; }
		public DateTime DateAdded { get; set; }

		[XmlArrayItem("Segment")]
		public List<ContentBucketSegment> Segments { get; set; }
	}
}
