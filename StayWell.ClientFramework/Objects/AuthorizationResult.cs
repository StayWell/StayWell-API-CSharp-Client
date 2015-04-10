namespace StayWell.ClientFramework.Objects
{
	public class AuthorizationResult
	{
		public AccessToken AccessToken { get; set; }
		public bool IsSuccessful { get; set; }
		public string ErrorDetails { get; set; }
	}
}