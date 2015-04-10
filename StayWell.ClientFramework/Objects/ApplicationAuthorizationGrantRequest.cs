namespace KswApi.Interface
{
	public class ApplicationAuthorizationGrantRequest
	{
		public string ApplicationId { get; set; }
		public string ApplicationSecret { get; set; }
		public string State { get; set; }
	}
}
