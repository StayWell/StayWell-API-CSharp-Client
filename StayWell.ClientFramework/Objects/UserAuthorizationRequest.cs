using System.Xml.Serialization;

namespace KswApi.Interface.Objects
{
	[XmlRoot("UserAuthorization")]
	public class UserAuthorizationRequest
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Scope { get; set; }
		public string State { get; set; }
	}
}
