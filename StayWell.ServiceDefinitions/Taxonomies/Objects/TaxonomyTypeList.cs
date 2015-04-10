using System;
using System.Xml.Serialization;
using StayWell.Interface;

namespace StayWell.ServiceDefinitions.Taxonomies.Objects
{
    [Serializable]
    [XmlRoot(ElementName = "TaxonomyTypes")]
    public class TaxonomyTypeList : ResultList<TaxonomyTypeResponse>
    {

    }
}
