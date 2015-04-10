using System;
using StayWell.ClientFramework.Internal;
using StayWell.ClientFramework.Objects;
using StayWell.ServiceDefinitions.Authorization;
using StayWell.ServiceDefinitions.Authorization.Objects;

namespace StayWell.ClientFramework
{
	public class OAuthClient
	{
		private static class OAuthGrantTypes
		{
			public const string REFRESH = "refresh_token";
			public const string USER = "authorization_code";
			public const string APPLICATION = "client_credentials";
		}

		private readonly string _serviceUri;
		private readonly IOAuthService _service;

		public OAuthClient()
			: this(ServiceClient.DEFAULT_API_URI)
		{
		}

		public OAuthClient(string serviceUri)
		{
			_serviceUri = serviceUri;

			ServiceProxy<IOAuthService> proxy = new ServiceProxy<IOAuthService>(serviceUri);

			_service = proxy.GetService();
		}

		public AccessToken RefreshUserToken(string refreshToken, string applicationId, string applicationSecret)
		{
			OAuthTokenRequest request = new OAuthTokenRequest
										{
											grant_type = OAuthGrantTypes.REFRESH,
											refresh_token = refreshToken,

											client_id = applicationId,
											client_secret = applicationSecret
										};

			OAuthTokenResponse response = _service.CreateAccessToken(request);

			return GetResponse(response, AccessTokenType.User);
		}

		public AccessToken GetAccessToken(string authorizationCode, string applicationId, string applicationSecret, string applicationUri = null)
		{
			OAuthTokenRequest request = new OAuthTokenRequest
										{
											grant_type = OAuthGrantTypes.USER,
											code = authorizationCode,
											client_id = applicationId,
											client_secret = applicationSecret,
											redirect_uri = applicationUri
										};

			OAuthTokenResponse response = _service.CreateAccessToken(request);

			return GetResponse(response, AccessTokenType.User);
		}

		public AccessToken GetApplicationToken(string applicationId, string applicationSecret, string state = null)
		{
			OAuthTokenRequest request = new OAuthTokenRequest
										{
											grant_type = OAuthGrantTypes.APPLICATION,
											client_id = applicationId,
											client_secret = applicationSecret,
											state = state
										};

			OAuthTokenResponse response = _service.CreateAccessToken(request);

			return GetResponse(response, AccessTokenType.Application);
		}

		private AccessToken GetResponse(OAuthTokenResponse response, AccessTokenType type)
		{
			DateTime now = DateTime.UtcNow;

			return new AccessToken
			{
				Token = response.access_token,
				AuthorizationServer = _serviceUri,
				ExpiresIn = response.expires_in,
				CreationTime = now,
				RefreshToken = response.refresh_token,
				Type = type
			};
		}
	}
}
