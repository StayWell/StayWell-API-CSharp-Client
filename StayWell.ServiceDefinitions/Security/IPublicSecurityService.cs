using System.ServiceModel;
using System.ServiceModel.Web;
using StayWell.Interface;
using StayWell.ServiceDefinitions.Security.Objects;

namespace StayWell.ServiceDefinitions.Security
{
	[ServiceContract(Name = "Security", Namespace = "http://www.kramesstaywell.com")]
	public interface IPublicSecurityService
	{
		[WebInvoke(UriTemplate = "Validations", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Public)]
		ValidationResponse ValidateAccessToken(string token);
	}
}
