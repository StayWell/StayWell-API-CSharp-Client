using System.ServiceModel;
using System.ServiceModel.Web;
using StayWell.Interface;
using StayWell.ServiceDefinitions.Authorization.Objects;

namespace StayWell.ServiceDefinitions.Authorization
{
	[ServiceContract(Name = "OAuth", Namespace = "http://www.kramesstaywell.com")]
	public interface IOAuthService
	{
		[WebInvoke(UriTemplate = "TokenEndpoint", Method = "POST")]
		[Allow(ClientType = ClientType.Public, Logging = AllowedLogging.NeverLog)]
		OAuthTokenResponse CreateAccessToken(OAuthTokenRequest request);

		[WebInvoke(UriTemplate = "ApplicationGrants", Method = "POST")]
		[Allow(ClientType = ClientType.Internal, Logging = AllowedLogging.NeverLog)]
		AuthorizationGrantResponse CreateApplicationAuthorizationGrant(ApplicationAuthorizationGrantRequest request);

		[WebInvoke(UriTemplate = "UserGrants", Method = "POST")]
		[Allow(ClientType = ClientType.Internal, Logging = AllowedLogging.NeverLog)]
		AuthorizationGrantResponse CreateUserAuthorizationGrant(UserAuthorizationRequest request);
	}
}
