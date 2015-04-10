using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using StayWell.ServiceDefinitions.Content.Objects;

namespace StayWell.ServiceDefinitions.Mapping.Objects
{
    // TODO: rename
    // TODO: remove inheritance
    [XmlType("Mapping")]
    public class ServiceLineMappingResponse : MappingResponse
    {
        //public Guid Id { get; set; }
        //public string Name { get; set; }
        //public string Uri { get; set; }
        //public string MappingType { get; set; }
        //public bool Published { get; set; }
        
        [Obsolete("Property not supported. Use class MappingResponse if Metadata is needed.", true)]
        public new List<ServiceLineResponse> Metadata { get; set; }
    }
}
