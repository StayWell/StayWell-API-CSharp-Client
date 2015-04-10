using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using StayWell.ServiceDefinitions.Common.Objects;

namespace StayWell.ServiceDefinitions.Security.Objects
{
	[XmlType("Validation")]
	public class ValidationResponse
	{
		public bool Valid { get; set; }
		public string State { get; set; }
		public List<string> Rights { get; set; }
		public UserResponse User { get; set; }
		public Guid? ApplicationId { get; set; }
	}
}
