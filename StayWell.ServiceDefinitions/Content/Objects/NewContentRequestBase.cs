using System.Xml.Serialization;
using StayWell.ServiceDefinitions.Content.Interfaces;

namespace StayWell.ServiceDefinitions.Content.Objects
{
    // TODO: fix naming...
    [XmlType("Content")]
	public class NewContentRequestBase : ContentArticleRequest, IContentCreateRequest
    {
        public string MasterId { get; set; }

        public string LanguageCode { get; set; }

        public string Slug { get; set; }
    }
}
