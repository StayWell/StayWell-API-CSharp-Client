using StayWell.ClientFramework.Enums;
using StayWell.ClientFramework.Interfaces;
using StayWell.ClientFramework.Internal;

namespace StayWell.ClientFramework
{
	public static class ClientFactory
	{
		public static TService GetClient<TService>(string applicationId, string applicationSecret)
		{
			return GetClient<TService>(ServiceClient.DEFAULT_API_URI, applicationId, applicationSecret, TokenStoreFactory.GetTokenStore());
		}

		public static TService GetClient<TService>(string serviceUri, string applicationId, string applicationSecret)
		{
			return GetClient<TService>(serviceUri, applicationId, applicationSecret, TokenStoreFactory.GetTokenStore());
		}

		public static TService GetClient<TService>(string serviceUri, string applicationId, string applicationSecret, TokenStoreType tokenStore)
		{
			return GetClient<TService>(serviceUri, applicationId, applicationSecret, TokenStoreFactory.GetTokenStore(tokenStore));
		}

		public static TService GetClient<TService>(string serviceUri, string applicationId, string applicationSecret, ITokenStore tokenStore)
		{
			ServiceProxy<TService> proxy = new ServiceProxy<TService>(serviceUri, applicationId, applicationSecret, tokenStore);

			return (TService)proxy.GetTransparentProxy();
		}
	}
}
