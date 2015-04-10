using System;

namespace StayWell.ClientFramework.Objects
{
	[Serializable]
	public class AccessToken
	{
		public string Token { get; set; }
		public string RefreshToken { get; set; }
		public string AuthorizationServer { get; set; }
		public int ExpiresIn { get; set; }
		public DateTime CreationTime { get; set; }
		public AccessTokenType Type { get; set; }
	}
}
