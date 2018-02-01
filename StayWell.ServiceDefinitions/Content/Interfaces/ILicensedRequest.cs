using System;

namespace StayWell.ServiceDefinitions.Content.Interfaces
{
	public interface ILicensedRequest
	{
		Guid? LicenseId { get; set; }
	}
}
