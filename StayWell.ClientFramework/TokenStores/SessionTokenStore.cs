using System;
using System.Web;
using StayWell.ClientFramework.Interfaces;
using StayWell.ClientFramework.Objects;

namespace StayWell.ClientFramework.TokenStores
{
	public class SessionTokenStore : ITokenStore
	{
		private const string SESSION_NAME_PREFIX = "KswApi.Token";

		public AccessToken GetToken(string applicationId)
		{
			if (HttpContext.Current == null)
				throw GetExceptionForInvalidContext();

            string sessionKey = GetSessionName(applicationId);

            return HttpContext.Current.Session[sessionKey] as AccessToken;
		}

        public void SetToken(AccessToken value, string applicationId)
		{
			if (HttpContext.Current == null)
				throw GetExceptionForInvalidContext();

            string sessionKey = GetSessionName(applicationId);

            HttpContext.Current.Session[sessionKey] = value;
		}

        public void RemoveToken(string applicationId)
		{
			if (HttpContext.Current == null)
				throw GetExceptionForInvalidContext();

            string sessionKey = SESSION_NAME_PREFIX + applicationId;

            HttpContext.Current.Session.Remove(sessionKey);
		}

        private string GetSessionName(string applicationId)
        {
            return string.IsNullOrEmpty(applicationId)
                ? SESSION_NAME_PREFIX
                : SESSION_NAME_PREFIX + "." + applicationId;
        }

		private Exception GetExceptionForInvalidContext()
		{
			return new InvalidOperationException("A client with a session token store can only be used with an active request context.");
		}
	}
}
