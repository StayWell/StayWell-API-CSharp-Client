using System;
using System.Xml.Serialization;
using StayWell.Interface;

namespace StayWell.ServiceDefinitions.Content.Objects
{
    [Serializable]
    [XmlRoot(ElementName = "ServiceLines")]
    public class ServiceLineResponseList : ResultList<ServiceLineResponse>
    {

    }

}
