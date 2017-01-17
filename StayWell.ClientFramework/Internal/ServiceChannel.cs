using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using Newtonsoft.Json;
using StayWell.ClientFramework.Enums;
using StayWell.ClientFramework.Helpers;
using StayWell.ClientFramework.Interfaces;
using StayWell.Interface;

namespace StayWell.ClientFramework.Internal
{
	internal class ServiceChannel : IServiceChannel
	{
		private const char CONTENT_TYPE_SEPARATOR = ';';
		private const string JSON_CONTENT_TYPE = "application/json";
		private const string FORM_CONTENT_TYPE = "application/x-www-form-urlencoded";
		private const string XML_CONTENT_TYPE = "application/xml";
		private const string HTML_CONTENT_TYPE = "text/html";
		private const int TIMEOUT_IN_MILLISECONDS = 300000;

		private readonly string _serviceUri;

		public ServiceChannel(string serviceUri)
		{
			_serviceUri = serviceUri;
		}

		public string GetMethodString(HttpMethod method)
		{
			switch (method)
			{
				case HttpMethod.Get:
					return HttpMethodString.GET;
				case HttpMethod.Post:
					return HttpMethodString.POST;
				case HttpMethod.Delete:
					return HttpMethodString.DELETE;
				case HttpMethod.Put:
					return HttpMethodString.PUT;
				default:
					throw new ArgumentOutOfRangeException(string.Format("Enum value {0} is not valid for HttpMethod in this context.", method));
			}
		}

		public object Invoke(OperationRequest operation)
		{
			WebRequest request = GetWebRequest(_serviceUri, operation);

			request.Method = GetMethodString(operation.HttpMethod);

			if (operation.Body != null && operation.FormParameters != null)
				throw new InvalidOperationException("Both body and form parameters cannot be supplied for an operation request.");

			if (operation.Body != null)
			{
				SerializeBody(request, operation.Body);
			}

			if (operation.FormParameters != null)
				SerializeForm(request, operation.FormParameters);

			try
			{
				if (operation.ResultType == typeof(StreamResponse))
				{
					WebResponse response = request.GetResponse();
					return new StreamResponse(response);
				}

				using (WebResponse response = request.GetResponse())
				{
					if (operation.ResultType == null || operation.ResultType == typeof(void))
						return null;

					using (Stream stream = response.GetResponseStream())
					{
						return Deserialize(operation.ResultType, stream, response.ContentType);
					}
				}
			}
			catch (WebException exception)
			{
				HandleWebException(exception, operation);

				throw;
			}
		}

		private void SerializeForm(WebRequest request, NameValueCollection formParameters)
		{
			request.ContentType = FORM_CONTENT_TYPE;

			if (formParameters.Count == 0)
				return;

			StringBuilder builder = new StringBuilder();

			bool first = true;

			foreach (string key in formParameters.AllKeys)
			{
				if (first)
					first = false;
				else
					builder.Append('&');

				builder.Append(Uri.EscapeDataString(key));
				builder.Append('=');
				builder.Append(Uri.EscapeDataString(formParameters[key] ?? string.Empty));
			}

			byte[] bytes = Encoding.UTF8.GetBytes(builder.ToString());

			request.ContentLength = bytes.Length;

			using (Stream stream = request.GetRequestStream())
			{
				stream.Write(bytes, 0, bytes.Length);
			}
		}

		private void HandleWebException(WebException exception, OperationRequest operation)
		{
			switch (operation.OperationType)
			{
				case OperationType.KswApi:
					HandleKswApiException(exception, operation);
					break;
				case OperationType.OAuth:
					HandleOAuthException(exception, operation);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void HandleOAuthException(WebException exception, OperationRequest operation)
		{
			OAuthError error;

			if (exception.Response == null)
				throw new ServiceException(HttpStatusCode.InternalServerError, exception.Message);

			using (Stream stream = exception.Response.GetResponseStream())
			{
				try
				{
					error = (OAuthError)Deserialize(typeof(OAuthError), stream, exception.Response.ContentType);
				}
				catch (SerializationException)
				{
					// if we can't deserialize a response, throw the original exception
					throw exception;
				}
			}

			if (error == null)
				throw new SerializationException("Unrecognized error format.");

			throw new OAuthException(((HttpWebResponse)exception.Response).StatusCode,
				GetOAuthErrorCode(error.error),
				error.error_description ?? "Error communicating with the server.");
		}

		private OAuthErrorCode GetOAuthErrorCode(string code)
		{
			switch (code)
			{
				case "invalid_application":
					return OAuthErrorCode.InvalidApplication;
				case "invalid_request":
					return OAuthErrorCode.InvalidRequest;
				case "invalid_grant":
					return OAuthErrorCode.InvalidGrant;
				case "unsupported_response_type":
					return OAuthErrorCode.UnsupportedResponseType;
				case "unauthorized_application":
					return OAuthErrorCode.UnauthorizedApplication;
				case "access_denied":
					return OAuthErrorCode.AccessDenied;
				case "unsupported_grant_type":
					return OAuthErrorCode.UnsupportedGrantType;
				case "invalid_scope":
					return OAuthErrorCode.InvalidScope;
				case "server_error":
					return OAuthErrorCode.ServerError;
				default:
					return OAuthErrorCode.UnknownError;
			}
		}

		private void HandleKswApiException(WebException exception, OperationRequest operation)
		{
			ErrorResponse error;

			if (exception.Response == null)
				throw new ServiceException(HttpStatusCode.InternalServerError, exception.Message);

			// handle the special case where the server is completely down
			HttpWebResponse httpResponse = (HttpWebResponse)exception.Response;
			if (!string.IsNullOrEmpty(httpResponse.ContentType) &&
				httpResponse.ContentType.StartsWith(HTML_CONTENT_TYPE) &&
				operation.ResultType.IsClass &&
				operation.ResultType != typeof(StreamResponse))
			{
				httpResponse.Close();
				throw new ServiceException(HttpStatusCode.ServiceUnavailable, "The service is down.");
			}

			using (Stream stream = exception.Response.GetResponseStream())
			{
				try
				{
					error = (ErrorResponse)Deserialize(typeof(ErrorResponse), stream, exception.Response.ContentType);
				}
				catch (SerializationException)
				{
					// if we can't deserialize a response, throw the original exception
					throw exception;
				}
			}

			if (error == null)
				throw new SerializationException("Unrecognized error format.");

			if (!string.IsNullOrEmpty(error.RedirectUri))
				throw new ServiceRedirectException((HttpStatusCode)error.StatusCode, error.RedirectUri, error.Details);

			throw new ServiceException((HttpStatusCode)error.StatusCode, error.Details, ExceptionResultType.Object, error.Data);
		}

		private void SerializeBody(WebRequest request, object body)
		{
			if (body is StreamRequest)
				SerializeStreamBody(request, (StreamRequest)body);
			else
				SerializeJsonBody(request, body);
		}

		private void SerializeStreamBody(WebRequest request, StreamRequest stream)
		{
			request.ContentType = !string.IsNullOrEmpty(stream.ContentType) ? stream.ContentType : "application/octet-stream";
			
			long length = stream.ContentLength >= 0 ? stream.ContentLength : stream.Length;

			Stream inStream = stream;

			if (length < 0)
			{
				MemoryStream ms = new MemoryStream();

				stream.CopyTo(ms);

				ms.Flush();

				length = ms.Length;

				ms.Position = 0;

				inStream = ms;
			}

			request.ContentLength = length;
			Stream outStream = request.GetRequestStream();

			inStream.CopyTo(outStream);
		}

		private void SerializeJsonBody(WebRequest request, object body)
		{
			// WebRequest stream requires content length to be accurate before
			// writing to the stream, otherwise it throws an exception, so we have
			// to figure out the length first
			request.ContentType = JSON_CONTENT_TYPE;

			JsonSerializer serializer = new JsonSerializer();

			MemoryStream ms = new MemoryStream();
			StreamWriter writer = new StreamWriter(ms);

			serializer.Serialize(writer, body);

			writer.Flush();

			byte[] bytes = ms.ToArray();

			request.ContentLength = bytes.Length;

			using (Stream stream = request.GetRequestStream())
			{
				stream.Write(bytes, 0, bytes.Length);
				stream.Flush();
			}
		}

		private WebRequest GetWebRequest(string baseUrl, OperationRequest operation)
		{
			string url = baseUrl;
			if (!UriHelper.HasScheme(url))
				url = UriHelper.AddHttpScheme(url);

			if (string.IsNullOrEmpty(operation.OperationName))
			{
				url = UriHelper.Combine(url, operation.ModuleName);
			}
			else if (operation.OperationName.StartsWith("/"))
			{
				// rooted path
				url = UriHelper.Combine(url, operation.OperationName);	
			}
			else
			{
				url = UriHelper.Combine(url, operation.ModuleName, operation.OperationName);
			}
			
			if (operation.QueryParameters != null)
				url = UriHelper.AddQuery(url, operation.QueryParameters);

			WebRequest request = WebRequest.Create(url);

			if (operation.Headers != null)
				request.Headers.Add(operation.Headers);

			request.ContentType = "";
			request.ContentLength = 0;

			request.Timeout = TIMEOUT_IN_MILLISECONDS;

			return request;
		}

		protected object Deserialize(Type type, Stream stream, string contentType)
		{
			if (contentType != null)
			{
				int index = contentType.IndexOf(CONTENT_TYPE_SEPARATOR);
				if (index != -1)
					contentType = contentType.Substring(0, index);
			}

			switch (contentType)
			{
				case JSON_CONTENT_TYPE:
					JsonSerializer jsonSerializer = new JsonSerializer();
					jsonSerializer.Error += (sender, args) => args.ErrorContext.Handled = true;
					return jsonSerializer.Deserialize(new StreamReader(stream), type);
				case XML_CONTENT_TYPE:
					if (type == typeof (string))
						return GetString(stream);

					return DeserializeXml(stream, type);
				default:
					throw new ServiceException(HttpStatusCode.InternalServerError, "Cannot communicate with the server. The server returned an unexpected content type: " + contentType + ".");
			}
		}

		private object GetString(Stream stream)
		{
			StreamReader reader = new StreamReader(stream);

			return reader.ReadToEnd();
		}

		private object DeserializeXml(Stream stream, Type type)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(type);
			return xmlSerializer.Deserialize(stream);
		}
	}
}
