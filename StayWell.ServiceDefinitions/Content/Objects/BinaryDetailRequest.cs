using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	[XmlType("Binary")]
	public class BinaryDetailRequest
	{
		public string Title { get; set; }
		public string Slug { get; set; }
		public string InvertedTitle { get; set; }

		[XmlArrayItem("AlternateTitle")]
		public List<string> AlternateTitles { get; set; }

		public string Blurb { get; set; }

		public DateTime? PostingDate { get; set; }

		public Guid? Replacement { get; set; }

		public string LegacyId { get; set; }

		[XmlArrayItem("Tag")]
		public List<string> Tags { get; set; }
		public List<CustomAttribute> CustomAttributes { get; set; }
	}
}
