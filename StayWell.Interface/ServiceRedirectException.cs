using System.Net;

namespace StayWell.Interface
{
	public class ServiceRedirectException : ServiceException
	{
		public ServiceRedirectException(HttpStatusCode statusCode, string redirect, string message) : base(statusCode, message)
		{
			RedirectUri = redirect;
		}

		public string RedirectUri { get; private set; }
	}
}
