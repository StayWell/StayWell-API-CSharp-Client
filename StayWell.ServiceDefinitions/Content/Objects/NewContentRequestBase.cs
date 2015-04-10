using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.Content.Objects
{
    // TODO: fix naming...
    [XmlType("Content")]
    public class NewContentRequestBase : ContentArticleRequest
    {
        public string MasterId { get; set; }

        public string LanguageCode { get; set; }

        public string Slug { get; set; }
    }
}
