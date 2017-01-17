using System;
using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.Common.Objects
{
	[XmlType("User")]
	public class UserResponse
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string FullName { get; set; }
		public string UserName { get; set; }
		public UserType Type { get; set; }
		public UserContainerType ContainerType { get; set; }
		public string Email { get; set; }
	}
}
