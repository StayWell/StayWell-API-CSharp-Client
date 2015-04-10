using System.Runtime.Serialization;

namespace StayWell.ServiceDefinitions.Mapping.Objects
{
	public class MappingServiceLineReportRequest : MappingServiceLineExportRequest
	{
		[DataMember(Name = "$skip")]
		public int Offset { get; set; }

		[DataMember(Name = "$top")]
		public int Count { get; set; }

		public MappingReportSort Sort { get; set; }
	}
}
