using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using StayWell.Interface;
using StayWell.ServiceDefinitions.Content.Objects;

namespace StayWell.ServiceDefinitions.Content
{
    [ServiceContract(Name = "Content", Namespace = "http://www.kramesstaywell.com")]
    public interface IPublicContentService
	{
		#region Articles
		[WebInvoke(UriTemplate = "", Method = "GET")]
        [OperationContract]
        [Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
        ContentList SearchContent(ContentSearchRequest request);

        [WebInvoke(UriTemplate = "{bucketIdOrSlug}/{idOrSlug}", Method = "GET")]
        [OperationContract]
        [Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
        ContentArticleResponse GetContent(string bucketIdOrSlug, string idOrSlug, GetContentOptions request);
     
		[WebInvoke(UriTemplate = "{bucketIdOrSlug}", Method = "POST")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content, Manage_Client_Content")]
		ContentMetadataResponse CreateContent(string bucketIdOrSlug, NewContentRequest content);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{idOrSlug}", Method = "PUT")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content, Manage_Client_Content")]
		ContentMetadataResponse UpdateContent(string bucketIdOrSlug, string idOrSlug, ContentArticleRequest content);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{idOrSlug}", Method = "DELETE")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content, Manage_Client_Content")]
		ContentResponse DeleteContent(string bucketIdOrSlug, string idOrSlug, Guid? licenseId, bool? publishedOnly = null);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Slugs", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp, Logging = AllowedLogging.NeverLog)]
		SlugResponse GetSlug(string bucketIdOrSlug, string title, string slug);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{idOrSlug}/Collections", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		AbbreviatedCollectionListResponse GetCollectionsForContent(string bucketIdOrSlug, string idOrSlug);

		[WebInvoke(UriTemplate = "Legacy/{bucketLegacyId}/{legacyId}", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		ContentArticleResponse GetLegacyContent(string bucketLegacyId, string legacyId, LegacyContentOptions options);

		#endregion

		#region Images

		// publicly accessible image using license id
		[WebInvoke(UriTemplate = "{licenseId}/{bucketIdOrSlug}/Images/{imageIdOrSlug}", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Public, Extensions = "jpg,jpeg,tiff,bmp,png,gif")]
		StreamResponse GetLicenseImage(string licenseId, string bucketIdOrSlug, string imageIdOrSlug);

		// publicly accessible image using application id
		[WebInvoke(UriTemplate = "ByApplication/{applicationId}/{bucketIdOrSlug}/Images/{imageIdOrSlug}", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Public, Extensions = "jpg,jpeg,tiff,bmp,png,gif")]
		StreamResponse GetApplicationImage(string applicationId, string bucketIdOrSlug, string imageIdOrSlug);

		#endregion

		#region History

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{contentIdOrSlug}/History", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
		ContentHistoryResponse GetHistory(string bucketIdOrSlug, string contentIdOrSlug, Guid? licenseId);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{contentIdOrSlug}/History/{historyId}", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
		ContentArticleResponse GetHistoryContent(string bucketIdOrSlug, string contentIdOrSlug, string historyId, Guid? licenseId, bool? includeBody);

		#endregion

		#region Taxonomies

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{contentIdOrSlug}/Taxonomies", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content")]
		TaxonomyListResponse SearchTaxonomies(string bucketIdOrSlug, string contentIdOrSlug);

		#endregion

		#region Binaries

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Binaries", Method = "POST")]
		[OperationContract]
		[Alternative(ContentType = "text/html")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content")]
		BinaryDetailResponse CreateBinary(string bucketIdOrSlug, StreamRequest body);

		// publicly accessible binary using license id
		[WebInvoke(UriTemplate = "{licenseId}/{bucketIdOrSlug}/Binaries/{binaryIdOrSlug}", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Public, Extensions = "jpg,jpeg,tiff,bmp,png,gif")]
		StreamResponse GetLicenseBinary(string licenseId, string bucketIdOrSlug, string binaryIdOrSlug);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Binaries/{binaryIdOrSlug}", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", Extensions = "jpg,jpeg,tiff,bmp,png,gif")]
		StreamResponse GetBinary(string bucketIdOrSlug, string binaryIdOrSlug);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Binaries/Details/{binaryIdOrSlug}", Method = "PUT")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content")]
		BinaryDetailResponse UpdateBinaryDetails(string bucketIdOrSlug, string binaryIdOrSlug, BinaryDetailRequest request);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Binaries/Details/{binaryIdOrSlug}", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
		BinaryDetailResponse GetBinaryDetails(string bucketIdOrSlug, string binaryIdOrSlug);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Binaries/Slugs/{idOrSlug}", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		SlugResponse GetBinarySlug(string bucketIdOrSlug, string idOrSlug, string slug, string title);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Binaries/{binaryIdOrSlug}", Method = "DELETE")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content")]
		BinaryDetailResponse DeleteBinary(string bucketIdOrSlug, string binaryIdOrSlug);

		#endregion

		#region Images

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Images", Method = "POST")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content")]
		[Alternative(ContentType = "text/html")]
		ImageDetailResponse CreateImage(string bucketIdOrSlug, StreamRequest body);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Images/{imageIdOrSlug}", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", Extensions = "jpg,jpeg,tiff,bmp,png,gif")]
		StreamResponse GetImage(string bucketIdOrSlug, string imageIdOrSlug);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Images/Details/{imageIdOrSlug}", Method = "PUT")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content")]
		ImageDetailResponse UpdateImageDetails(string bucketIdOrSlug, string imageIdOrSlug, ImageDetailRequest request);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Images/Details/{imageIdOrSlug}", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
		ImageDetailResponse GetImageDetails(string bucketIdOrSlug, string imageIdOrSlug);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Images/Slugs/{idOrSlug}", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		SlugResponse GetImageSlug(string bucketIdOrSlug, string idOrSlug, string slug, string title);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Images/{imageIdOrSlug}", Method = "DELETE")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content")]
		ImageDetailResponse DeleteImage(string bucketIdOrSlug, string imageIdOrSlug);

		#endregion
	}
}
