using System.ServiceModel;
using System.ServiceModel.Web;
using StayWell.Interface;
using StayWell.ServiceDefinitions.Buckets.Objects;

namespace StayWell.ServiceDefinitions.Buckets
{
    [ServiceContract(Name = "Buckets", Namespace = "http://www.kramesstaywell.com")]
    public interface IPublicBucketService
    {
        [WebInvoke(UriTemplate = "", Method = "GET")]
        [Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
        ContentBucketList SearchBuckets(BucketSearchRequest request);

        [WebInvoke(UriTemplate = "{idOrSlug}", Method = "GET")]
        [Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
        ContentBucketResponse GetBucket(string idOrSlug);

		[WebInvoke(UriTemplate = "", Method = "POST")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content")]
		ContentBucketResponse CreateBucket(ContentBucketCreateRequest bucket);

		[WebInvoke(UriTemplate = "Legacy/{legacyId}", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		ContentBucketResponse GetLegacyBucket(string legacyId);

		[WebInvoke(UriTemplate = "{idOrSlug}", Method = "PUT")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content")]
		ContentBucketResponse UpdateBucket(string idOrSlug, ContentBucketUpdateRequest bucket);

		[WebInvoke(UriTemplate = "{idOrSlug}", Method = "DELETE")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content")]
		ContentBucketResponse DeleteBucket(string idOrSlug);

		[WebInvoke(UriTemplate = "Slugs", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp, Logging = AllowedLogging.NeverLog)]
		SlugResponse GetSlug(string title, string slug);

    }
}
