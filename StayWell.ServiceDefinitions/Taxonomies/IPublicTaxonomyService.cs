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
        TaxonomyTypeList SearchTaxonomyTypes();

        [WebInvoke(UriTemplate = "{slug}", Method = "GET")]
        [OperationContract]
        [Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
        TaxonomyTypeResponse GetTaxonomyType(string slug);
    }
}
