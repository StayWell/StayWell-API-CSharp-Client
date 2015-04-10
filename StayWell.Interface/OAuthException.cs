using System;
using System.Net;

namespace StayWell.Interface
{
    [Serializable]
    public class OAuthException : Exception
    {
        public OAuthException(HttpStatusCode httpStatusCode, OAuthErrorCode oauthErrorCode, string errorDescription)
            : base(errorDescription)
        {
            HttpStatusCode = httpStatusCode;
            ErrorCode = oauthErrorCode;
            ErrorDescription = errorDescription;
        }

        public OAuthErrorCode ErrorCode { get; private set; }
        public HttpStatusCode HttpStatusCode { get; private set; }
        public string ErrorDescription { get; private set; }
    }
}
