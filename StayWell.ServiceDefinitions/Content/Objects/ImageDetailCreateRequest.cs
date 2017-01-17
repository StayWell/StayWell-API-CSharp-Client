using System;
using System.Xml.Serialization;
using StayWell.ServiceDefinitions.Buckets.Objects;
using StayWell.ServiceDefinitions.Content.Interfaces;

namespace StayWell.ServiceDefinitions.Content.Objects
{

	[XmlType("Image")]
	public class ImageDetailCreateRequest : ImageDetailRequest, IContentCreateRequest
	{
		public Guid Id { get; set; }
		public FormatType Format { get; set; }

		// set in code instead?
		public FileStatus Status { get; set; }

		public string MasterId { get; set; }

		public string LanguageCode { get; set; }

		public int Width { get; set; }
		public int Height { get; set; }
		public byte[] Hash { get; set; }
	}
}
