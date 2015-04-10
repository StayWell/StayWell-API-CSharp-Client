using System.Xml.Serialization;
using StayWell.Interface;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	[XmlType("History")]
	public class ContentHistoryResponse : ResultList<ContentHistoryItemResponse>
	{
	}
}
