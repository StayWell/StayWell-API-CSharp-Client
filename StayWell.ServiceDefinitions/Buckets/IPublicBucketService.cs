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
		[Document("Search buckets")]
        ContentBucketList SearchBuckets(BucketSearchRequest request);

        [WebInvoke(UriTemplate = "{idOrSlug}", Method = "GET")]
        [Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		[Document("Get the metadata for a bucket")]
        ContentBucketResponse GetBucket(string idOrSlug);

		[WebInvoke(UriTemplate = "", Method = "POST")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content")]
		[Document("Create a bucket")]
		ContentBucketResponse CreateBucket(ContentBucketCreateRequest bucket);

		[WebInvoke(UriTemplate = "Legacy/{legacyId}", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		[Document("Get the metadata for a bucket using its legacy UCR ID")]
		ContentBucketResponse GetLegacyBucket(string legacyId);

		[WebInvoke(UriTemplate = "{idOrSlug}", Method = "PUT")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content")]
		[Document("Update a bucket")]
		ContentBucketResponse UpdateBucket(string idOrSlug, ContentBucketUpdateRequest bucket);

		[WebInvoke(UriTemplate = "{idOrSlug}", Method = "DELETE")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content")]
		[Document("Delete a bucket")]
		ContentBucketResponse DeleteBucket(string idOrSlug);

		[WebInvoke(UriTemplate = "Slugs", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp, Logging = AllowedLogging.NeverLog)]
		[Document("Get a valid slug for a bucket")]
		SlugResponse GetSlug(string title, string slug);

    }
}
