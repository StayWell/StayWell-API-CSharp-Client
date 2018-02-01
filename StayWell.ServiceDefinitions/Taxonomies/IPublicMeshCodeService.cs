using System.ServiceModel;
using System.ServiceModel.Web;
using StayWell.Interface;
using StayWell.ServiceDefinitions.Taxonomies.Objects;

namespace StayWell.ServiceDefinitions.Taxonomies
{
	[ServiceContract(Name = "MeshCodes", Namespace = "http://www.kramesstaywell.com")]
	public interface IPublicMeshCodeService
	{
		[WebInvoke(UriTemplate = "", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		[Document("Search MeSH codes")]
		MeshCodeResponseList SearchMeshCodes(PagedSearchRequest request);

		[WebInvoke(UriTemplate = "{meshCode}", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		[Document("Get details for a specific MeSH code")]
		MeshCodeResponse GetMeshCode(string meshCode);
	}
}
