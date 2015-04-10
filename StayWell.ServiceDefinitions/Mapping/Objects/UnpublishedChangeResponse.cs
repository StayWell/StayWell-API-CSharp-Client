using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.Mapping.Objects
{
	[XmlType("Unpublished")]
	public class UnpublishedChangeResponse
	{
		public int PreviouslyUnpublished { get; set; }
		public int NewlyUnpublished { get; set; }
		public int Total { get; set; }
	}
}
