using System.Xml.Serialization;
using StayWell.Interface;

namespace StayWell.ServiceDefinitions.Collections.Objects
{
    [XmlRoot("Collections")]
    public class CollectionListResponse : PagedResultList<CollectionResponse>
    {

    }
}

