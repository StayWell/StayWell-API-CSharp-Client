using System.ServiceModel;
using System.ServiceModel.Web;
using KswApi.Interface.Attributes;
using KswApi.Interface.Enums;
using KswApi.Interface.Objects;

namespace KswApi.Interface
{
	[ServiceContract(Name = "OAuth", Namespace = "http://www.kramesstaywell.com")]
	public interface IOAuthService
	{
		[WebInvoke(UriTemplate = "TokenEndpoint", Method = "POST")]
		[Allow(ClientType = ClientType.Public, Logging = AllowedLogging.LogWithoutBody)]
		OAuthTokenResponse CreateAccessToken(OAuthTokenRequest request);

		[WebInvoke(UriTemplate = "ApplicationGrants", Method = "POST")]
		[Allow(ClientType = ClientType.Internal, Logging = AllowedLogging.LogWithoutBody)]
		AuthorizationGrantResponse CreateApplicationAuthorizationGrant(ApplicationAuthorizationGrantRequest request);

		[WebInvoke(UriTemplate = "UserGrants", Method = "POST")]
		[Allow(ClientType = ClientType.Internal, Logging = AllowedLogging.LogWithoutBody)]
		AuthorizationGrantResponse CreateUserAuthorizationGrant(UserAuthorizationRequest request);
	}
}
