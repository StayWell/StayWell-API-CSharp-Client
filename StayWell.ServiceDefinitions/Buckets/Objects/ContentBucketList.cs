using System.Xml.Serialization;
using StayWell.Interface;

namespace StayWell.ServiceDefinitions.Buckets.Objects
{
	[XmlRoot("Buckets")]
	public class ContentBucketList : PagedResultList<ContentBucketResponse>
	{

	}
}
