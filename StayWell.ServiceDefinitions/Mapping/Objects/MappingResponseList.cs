using System;
using System.Xml.Serialization;
using StayWell.Interface;

namespace StayWell.ServiceDefinitions.Mapping.Objects
{
    [XmlType("Mappings")]
    [Obsolete("Deprecated. Use ServiceLineMappingResponseList instead.")]
    public class MappingResponseList : ResultList<ServiceLineMappingResponse>
    {

    }
}
