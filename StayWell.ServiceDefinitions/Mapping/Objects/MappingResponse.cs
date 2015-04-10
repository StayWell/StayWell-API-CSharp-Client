using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using StayWell.ServiceDefinitions.Content.Objects;

namespace StayWell.ServiceDefinitions.Mapping.Objects
{
	[XmlType("Mapping")]
	public class MappingResponse
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Uri { get; set; }
		public string MappingType { get; set; }
		public bool Published { get; set; }
        public List<ServiceLineResponse> Metadata { get; set; }
	}
}
