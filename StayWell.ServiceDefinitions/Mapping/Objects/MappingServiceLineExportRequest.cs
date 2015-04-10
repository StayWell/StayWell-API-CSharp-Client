using System;

namespace StayWell.ServiceDefinitions.Mapping.Objects
{
	public class MappingServiceLineExportRequest
	{
		public Guid? LicenseId { get; set; }
		public string Type { get; set; }
		public string Audience { get; set; }
        public bool IsDelineated { get; set; }
	}
}
