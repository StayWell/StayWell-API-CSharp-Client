using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using StayWell.Interface;
using StayWell.ServiceDefinitions.Collections.Objects;

namespace StayWell.ServiceDefinitions.Collections
{
	[ServiceContract(Name = "Collections", Namespace = "http://www.kramesstaywell.com")]
	public interface IPublicCollectionService
	{
		[WebInvoke(UriTemplate = "", Method = "POST")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content")]
		[Document("Create a collection")]
		CollectionResponse CreateCollection(CollectionCreateRequest collection);

		[WebInvoke(UriTemplate = "", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		[Document("Search collections")]
		CollectionListResponse SearchCollections(CollectionSearchRequest request);

		[WebInvoke(UriTemplate = "{idOrSlug}", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		[Document("Get a collection")]
		CollectionResponse GetCollection(string idOrSlug, bool? includeChildren, bool? recursive, bool? includeContent, Guid? licenseId, bool includeBlockedContent = false, bool getOriginal = false);

		[WebInvoke(UriTemplate = "{idOrSlug}", Method = "PUT")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content")]
		[Document("Update a collection")]
		CollectionResponse UpdateCollection(
			[Document("Unique identifier")] string idOrSlug,
			CollectionRequest collection);

		[WebInvoke(UriTemplate = "{idOrSlug}", Method = "DELETE")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content")]
		[Document("Delete a collection")]
		CollectionResponse DeleteCollection(string idOrSlug);

		[WebInvoke(UriTemplate = "Slugs", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp, Logging = AllowedLogging.NeverLog)]
		[Document("Get a valid slug for a collection")]
		SlugResponse GetSlug(string title, string slug);

		[WebInvoke(UriTemplate = "Legacy/{legacyId}", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		[Document("Get a collection by its legacy UCR ID")]
		CollectionResponse GetLegacyCollection(string legacyId, bool? includeChildren, bool? recursive, bool? includeContent, Guid? licenseId);
	}
}
