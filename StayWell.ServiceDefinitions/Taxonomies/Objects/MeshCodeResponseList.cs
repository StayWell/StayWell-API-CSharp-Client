using System.Xml.Serialization;
using StayWell.Interface;

namespace StayWell.ServiceDefinitions.Taxonomies.Objects
{
	[XmlRoot(ElementName = "MeshCodes")]
	public class MeshCodeResponseList : PagedResultList<MeshCodeResponse>
	{

	}
}
