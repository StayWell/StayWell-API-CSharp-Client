using System;
using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	[XmlType("History")]
	public class ContentHistoryItemResponse
	{
		public Guid Id { get; set; }
		public Guid ContentId { get; set; }
		public ContentRevisionType RevisionType { get; set; }
		public DateTime Time { get; set; }
	}
}
