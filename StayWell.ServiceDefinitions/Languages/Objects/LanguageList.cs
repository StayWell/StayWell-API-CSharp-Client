using System.Xml.Serialization;
using StayWell.Interface;
using StayWell.ServiceDefinitions.Content.Objects;

namespace StayWell.ServiceDefinitions.Languages.Objects
{
	[XmlType("Languages")]
	public class LanguageList : ResultList<Language>
	{
	}
}
