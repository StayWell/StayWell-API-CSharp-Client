using System.Collections.Concurrent;
using StayWell.ClientFramework.Interfaces;
using StayWell.ClientFramework.Objects;

namespace StayWell.ClientFramework.TokenStores
{
	public class GlobalApplicationStore : ITokenStore
	{
		private static readonly ConcurrentDictionary<string, AccessToken> Tokens = new ConcurrentDictionary<string, AccessToken>();

		public AccessToken GetToken(string applicationId)
		{
			AccessToken token;

			Tokens.TryGetValue(applicationId, out token);

			return token;
		}

		public void SetToken(AccessToken value, string applicationId)
		{
			Tokens[applicationId] = value;
		}

		public void RemoveToken(string applicationId)
		{
			AccessToken token;

			Tokens.TryRemove(applicationId, out token);
		}
	}
}
