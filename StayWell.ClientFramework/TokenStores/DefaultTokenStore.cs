using StayWell.ClientFramework.Interfaces;
using StayWell.ClientFramework.Objects;

namespace StayWell.ClientFramework.TokenStores
{
	internal class DefaultTokenStore : ITokenStore
	{
		private AccessToken _token;

		public AccessToken GetToken(string applicationId = null)
		{
			return _token;
		}

        public void SetToken(AccessToken value, string applicationId)
		{
			_token = value;
		}

        public void RemoveToken(string applicationId)
		{
			_token = null;
		}
	}
}
