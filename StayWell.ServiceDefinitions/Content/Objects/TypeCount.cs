using System.Collections.Generic;
using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	public class TypeCount
	{
		[XmlArrayItem("Type")]
		public Dictionary<FormatType, int> Types { get; set; }

		public int Other { get; set; }

		public int Missing { get; set; }

		public int Total { get; set; }
	}
}
