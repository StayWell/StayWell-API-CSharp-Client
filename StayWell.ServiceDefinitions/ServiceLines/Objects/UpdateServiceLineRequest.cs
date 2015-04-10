using System.Collections.Generic;
using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.ServiceLines.Objects
{
	public class UpdateServiceLineRequest
	{
		[XmlArrayItem("Keyword")]
		public List<string> Keywords { get; set; }
	}
}
