// ReSharper disable InconsistentNaming

using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.Authorization.Objects
{
	[XmlRoot("token")]
	public class OAuthTokenRequest
	{
		public string client_id { get; set; }
		public string client_secret { get; set; }
		public string grant_type { get; set; }
		public string code { get; set; }
		public string redirect_uri { get; set; }
		public string refresh_token { get; set; }
		public string scope { get; set; }
		
		// state is not an official part of the OAuth spec:
		// it is a non-breaking extension to facilitate
		// client needs
		public string state { get; set; }
	}
}
