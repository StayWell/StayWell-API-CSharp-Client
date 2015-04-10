using System;
using System.Net;

namespace StayWell.Interface
{
	public class ServiceException : Exception
	{
		/// <summary>
		/// Serivce Exception 
		/// </summary>
		/// <param name="statusCode">HttpStatusCode </param>
		/// <param name="message">Exception Message</param>
		/// <param name="resultType">A result type to pass back (allows image/html/document error results)</param>
		/// <param name="data">Additional data</param>
		public ServiceException(HttpStatusCode statusCode, string message, ExceptionResultType resultType = ExceptionResultType.Object, ErrorData data = null)
			: base(message)
		{
			StatusCode = statusCode;
            Title = string.Format("Service Exception: {0}", Enum.GetName(typeof(HttpStatusCode), statusCode));
			ResultType = resultType;
			ErrorData = data;
		}

		public HttpStatusCode StatusCode { get; private set; }
        public string Title { get; private set; }
		public ExceptionResultType ResultType { get; private set; }
		public ErrorData ErrorData { get; set; }
	}
}
