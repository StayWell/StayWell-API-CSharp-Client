using System;
using System.Web;
using StayWell.ClientFramework.Enums;
using StayWell.ClientFramework.Interfaces;
using StayWell.ClientFramework.TokenStores;

namespace StayWell.ClientFramework.Internal
{
	internal static class TokenStoreFactory
	{
		public static ITokenStore GetTokenStore(TokenStoreType tokenStoreType)
		{
			switch (tokenStoreType)
			{
				case TokenStoreType.Session:
					return new SessionTokenStore();
				case TokenStoreType.Cookie:
					return new CookieTokenStore();
				case TokenStoreType.SingleApplication:
					return new GlobalApplicationStore();
				case TokenStoreType.PerClient:
					return new DefaultTokenStore();
				default:
					throw new ArgumentOutOfRangeException("tokenStoreType");
			}
		}

		public static ITokenStore GetTokenStore()
		{
			if (HttpContext.Current != null)
				return GetTokenStore(TokenStoreType.Cookie);

			return GetTokenStore(TokenStoreType.PerClient);
		}
	}
}
