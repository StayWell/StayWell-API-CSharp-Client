using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	public class ModifiedContentResponse
	{
		public DateTime StartTime { get; set; }

		[XmlArrayItem("Addition")]
		public List<ContentItemResponse> Additions { get; set; }

		[XmlArrayItem("Removal")]
		public List<ContentItemResponse> Removals { get; set; }

		[XmlArrayItem("Update")]
		public List<ContentItemResponse> Updates { get; set; }
	}
}
