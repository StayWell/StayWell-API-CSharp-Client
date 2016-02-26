using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace StayWell.WebExample.Models
{
    [Serializable]
    public class Category
    {
        [XmlAttribute("Slug")]
        public string Slug { get; set; }
        [XmlArray]
        public List<Name> Names { get; set; }
        [XmlArray]
        public List<Bucket> Buckets { get; set; }
        [XmlArray]
        public List<ServiceLine> ServiceLines { get; set; }
        [XmlArray]
        public List<Collection> Collections { get; set; }
    }
    [Serializable]
    public class Name
    {
        [XmlAttribute("LanguageCode")]
        public string LanguageCode { get; set; }
        [XmlText]
        public string Value { get; set; }
    }
    [Serializable]
    public class Bucket
    {
        [XmlAttribute("Slug")]
        public string Slug { get; set; }
    }
    [Serializable]
    public class ServiceLine
    {
        [XmlAttribute("Slug")]
        public string Slug { get; set; }
    }
    [Serializable]
    public class Collection
    {
        [XmlAttribute("Slug")]
        public string Slug { get; set; }
         [XmlAttribute("Name")]
        public string Name { get; set; }
    }
}