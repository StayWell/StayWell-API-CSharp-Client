using System.Xml.Serialization;
using StayWell.Interface;

namespace StayWell.ServiceDefinitions.Mapping.Objects
{
    // TODO: rename
    // TODO: kill MappingResponseList
    [XmlType("ServiceLineMappings")]
    // public class ServiceLineMappingResponseList : MappingResponseList
    public class ServiceLineMappingResponseList : ResultList<MappingResponse>
    {
    }
}
