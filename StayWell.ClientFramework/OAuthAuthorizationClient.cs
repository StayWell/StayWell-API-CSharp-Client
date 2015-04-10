using System;
using StayWell.ClientFramework.Enums;
using StayWell.ClientFramework.Interfaces;
using StayWell.ClientFramework.Internal;
using StayWell.ClientFramework.Objects;
using StayWell.Interface;

namespace StayWell.ClientFramework
{
	public class OAuthAuthorizationClient
	{
		public const int REFRESH_TOKEN_EXPIRATION_IN_HOURS = 23; // Actual expiration is 24 hours, expire after 23 on the client side to be sure

		private readonly ITokenStore _tokenStore;
		private readonly string _serviceUri;
		private readonly string _applicationId;
		private readonly string _applicationSecret;

		public OAuthAuthorizationClient(string serviceUri, string applicationId, string applicationSecret, ITokenStore tokenStore)
		{
			_serviceUri = serviceUri;
			_tokenStore = tokenStore;
			_applicationId = applicationId;
			_applicationSecret = applicationSecret;
		}

		public OAuthAuthorizationClient(string serviceUri, string applicationId, string applicationSecret)
			: this(serviceUri, applicationId, applicationSecret, TokenStoreFactory.GetTokenStore())
		{
		}

		public OAuthAuthorizationClient(string serviceUri, string applicationId, string applicationSecret, TokenStoreType type)
			: this(serviceUri, applicationId, applicationSecret, TokenStoreFactory.GetTokenStore(type))
		{
		}

		public AuthorizationResult Authorize(string authorizationCode, string redirectUri = null)
		{
			OAuthClient client = new OAuthClient(_serviceUri);

			try
			{
				AccessToken accessToken = client.GetAccessToken(authorizationCode, _applicationId, _applicationSecret, redirectUri);

				_tokenStore.SetToken(accessToken, _applicationId);

				return new AuthorizationResult
						   {
							   IsSuccessful = true,
							   AccessToken = accessToken
						   };
			}
			catch (OAuthException exception)
			{
				return new AuthorizationResult
						   {
							   IsSuccessful = false,
							   ErrorDetails = exception.ErrorDescription
						   };
			}
		}

		public void Unauthorize()
		{
			_tokenStore.RemoveToken(_applicationId);
		}

		public bool IsAuthorized
		{
			get
			{
				AccessToken token = _tokenStore.GetToken(_applicationId);
				if (token == null)
					return false;

				DateTime now = DateTime.UtcNow;

				if (now < token.CreationTime + TimeSpan.FromSeconds(token.ExpiresIn))
					return true;

				// if we have a refresh token
				if (!string.IsNullOrEmpty(token.RefreshToken) && now < token.CreationTime + TimeSpan.FromHours(REFRESH_TOKEN_EXPIRATION_IN_HOURS))
					return true;

				return false;
			}
		}
	}
}
