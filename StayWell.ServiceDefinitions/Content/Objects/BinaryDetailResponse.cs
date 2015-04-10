using System;
using System.Xml.Serialization;
using StayWell.ServiceDefinitions.Buckets.Objects;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	[XmlType("Binary")]
	public class BinaryDetailResponse : BinaryDetailRequest
	{
		public Guid Id { get; set; }
		
		public ContentBucketReference Bucket { get; set; }

		public string Uri { get; set; }
		public FormatType Format { get; set; }
		public DateTime DateAdded { get; set; }
		public DateTime DateModified { get; set; }
        public string OriginName { get; set; }
	}
}
