using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.ServiceLines.Objects
{
	[XmlRoot("ServiceLine")]
	public class CreateServiceLineRequest : UpdateServiceLineRequest
	{
		public string Name { get; set; }
	}
}
