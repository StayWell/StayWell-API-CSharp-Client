using System.Net;
using Staywell.ClientFramework.Exceptions;

namespace KswApi.Interface.Exceptions
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
