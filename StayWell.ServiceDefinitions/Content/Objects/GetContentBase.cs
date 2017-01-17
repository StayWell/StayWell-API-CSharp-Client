using System;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	public class GetContentBase
	{
		public bool IncludeBody { get; set; }
		public bool Draft { get; set; }
		public bool GetOriginal { get; set; }
		public Guid? LicenseId { get; set; }
        public bool IncludeBlockedContent { get; set; }
    }
}
