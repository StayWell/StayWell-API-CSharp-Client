using System;
using System.Collections.Specialized;
using StayWell.ClientFramework.Enums;

namespace StayWell.ClientFramework.Internal
{
	internal class OperationRequest
	{
		public OperationType OperationType { get; set; }
		public HttpMethod HttpMethod { get; set; }
		public Type ResultType { get; set; }
		public string ModuleName { get; set; }
		public string OperationName { get; set; }
		public object Body { get; set; }
		public NameValueCollection QueryParameters { get; set; }
		public NameValueCollection Headers { get; set; }
		public NameValueCollection FormParameters { get; set; }
	}
}
