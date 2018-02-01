using System.ServiceModel;
using System.ServiceModel.Web;
using StayWell.Interface;
using StayWell.ServiceDefinitions.Taxonomies.Objects;

namespace StayWell.ServiceDefinitions.Taxonomies
{
    [ServiceContract(Name = "Taxonomies", Namespace = "http://www.kramesstaywell.com")]
    public interface IPublicTaxonomyService
    {
        [WebInvoke(UriTemplate = "", Method = "GET")]
        [OperationContract]
        [Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		[Document("Get all content taxonomy types")]
        TaxonomyTypeList SearchTaxonomyTypes();

        [WebInvoke(UriTemplate = "{slug}", Method = "GET")]
        [OperationContract]
        [Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		[Document("Get the metadata for a taxonomy type by slug")]
        TaxonomyTypeResponse GetTaxonomyType(string slug);
    }
}
