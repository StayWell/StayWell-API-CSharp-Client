using System.Net;
using System.Web;

namespace KswApi.Interface.Objects
{
	public class ErrorResponse
	{
		public ErrorResponse()
		{
		}

		public ErrorResponse(HttpStatusCode code, string message, ErrorData data = null)
		{
			StatusCode = (int)code;
			StatusDescription = HttpWorkerRequest.GetStatusDescription(StatusCode);
			Details = message;
			Data = data;
		}

		public int StatusCode { get; set; }
		public string StatusDescription { get; set; }
		public string Details { get; set; }
		public string RedirectUri { get; set; }
		public ErrorData Data { get; set; }
	}
}