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

		// this isn't very rest-y
		[WebInvoke(UriTemplate = "RelatedContent", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		RelatedContentList SearchRelatedContent(RelatedContentSearchRequest request);

		[WebInvoke(UriTemplate = "", Method = "GET")]
        [Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
        ContentList SearchContent(ContentSearchRequest request);

        [WebInvoke(UriTemplate = "{bucketIdOrSlug}/{idOrSlug}", Method = "GET")]
        [Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
        ContentArticleResponse GetContent(string bucketIdOrSlug, string idOrSlug, GetContentOptions request);
     
		[WebInvoke(UriTemplate = "{bucketIdOrSlug}", Method = "POST")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content, Manage_Client_Content, Manage_License_Content")]
		ContentArticleResponse CreateContent(string bucketIdOrSlug, NewContentRequest content);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{idOrSlug}", Method = "PUT")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content, Manage_Client_Content, Manage_License_Content")]
		ContentArticleResponse UpdateContent(string bucketIdOrSlug, string idOrSlug, ContentArticleRequest content);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{idOrSlug}", Method = "DELETE")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content, Manage_Client_Content, Manage_License_Content")]
		ContentResponse DeleteContent(string bucketIdOrSlug, string idOrSlug, Guid? licenseId, bool? publishedOnly = null);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Slugs", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp, Logging = AllowedLogging.NeverLog)]
		SlugResponse GetSlug(string bucketIdOrSlug, string title, string slug);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{idOrSlug}/Collections", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		AbbreviatedCollectionListResponse GetCollectionsForContent(string bucketIdOrSlug, string idOrSlug);

		[WebInvoke(UriTemplate = "Legacy/{bucketLegacyId}/{legacyId}", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		ContentArticleResponse GetLegacyContent(string bucketLegacyId, string legacyId, LegacyContentOptions options);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{idOrSlug}/Rating", Method = "DELETE")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Client_Content, Manage_License_Content, Manage_Content")]
		ContentRatingResponse ResetRating(string bucketIdOrSlug, string idOrSlug, Guid? licenseId);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{idOrSlug}/ViewCount", Method = "DELETE")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Client_Content, Manage_License_Content, Manage_Content")]
		ContentRatingResponse ResetViewCount(string bucketIdOrSlug, string idOrSlug, Guid? licenseId);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{idOrSlug}/Rating", Method = "POST")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
		ContentRatingResponse AddRating(string bucketIdOrSlug, string idOrSlug, RatingRequest theRequest);

		#endregion

		#region Images

		// publicly accessible image using license id
		[WebInvoke(UriTemplate = "{licenseId}/{bucketIdOrSlug}/Images/{imageIdOrSlug}", Method = "GET")]
		[Allow(ClientType = ClientType.Public, Extensions = "jpg,jpeg,tiff,bmp,png,gif")]
		StreamResponse GetLicenseImage(string licenseId, string bucketIdOrSlug, string imageIdOrSlug);

		// publicly accessible image using application id
		[WebInvoke(UriTemplate = "ByApplication/{applicationId}/{bucketIdOrSlug}/Images/{imageIdOrSlug}", Method = "GET")]
		[Allow(ClientType = ClientType.Public, Extensions = "jpg,jpeg,tiff,bmp,png,gif")]
		StreamResponse GetApplicationImage(string applicationId, string bucketIdOrSlug, string imageIdOrSlug);

		#endregion

		#region History

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{contentIdOrSlug}/History", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
		ContentHistoryResponse GetHistory(string bucketIdOrSlug, string contentIdOrSlug, Guid? licenseId);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{contentIdOrSlug}/History/{historyId}", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
		ContentArticleResponse GetHistoryContent(string bucketIdOrSlug, string contentIdOrSlug, string historyId, Guid? licenseId, bool? includeBody);

		#endregion

		#region Taxonomies

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{contentIdOrSlug}/Taxonomies", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content")]
		TaxonomyListResponse SearchTaxonomies(string bucketIdOrSlug, string contentIdOrSlug);

		#endregion

		#region Binaries

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Binaries", Method = "POST")]
		[Alternative(ContentType = "text/html")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content, Manage_Client_Content, Manage_License_Content")]
		BinaryDetailResponse CreateBinary(string bucketIdOrSlug, StreamRequest body, string licenseId = null);

		// publicly accessible binary using license id
		[WebInvoke(UriTemplate = "{licenseId}/{bucketIdOrSlug}/Binaries/{binaryIdOrSlug}", Method = "GET")]
		[Allow(ClientType = ClientType.Public, Extensions = "jpg,jpeg,tiff,bmp,png,gif")]
		StreamResponse GetLicenseBinary(string licenseId, string bucketIdOrSlug, string binaryIdOrSlug);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Binaries/{binaryIdOrSlug}", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", Extensions = "jpg,jpeg,tiff,bmp,png,gif")]
		StreamResponse GetBinary(string bucketIdOrSlug, string binaryIdOrSlug);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Binaries/Details/{binaryIdOrSlug}", Method = "PUT")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content, Manage_Client_Content, Manage_License_Content")]
		BinaryDetailResponse UpdateBinaryDetails(string bucketIdOrSlug, string binaryIdOrSlug, BinaryDetailRequest request);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Binaries/Details/{binaryIdOrSlug}", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
		BinaryDetailResponse GetBinaryDetails(string bucketIdOrSlug, string binaryIdOrSlug, GetContentOptions options);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Binaries/Details/Slugs", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		SlugResponse GetBinarySlug(string bucketIdOrSlug, string slug, string title);

		// prev. returned BinaryDetailResponse <== return specific types for image, binary, streaming media?
		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Binaries/{binaryIdOrSlug}", Method = "DELETE")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content, Manage_Client_Content, Manage_License_Content")]
		ContentResponse DeleteBinary(string bucketIdOrSlug, string binaryIdOrSlug, Guid? licenseId, bool? publishedOnly = null);

/*
		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Binaries/{idOrSlug}/Rating", Method = "DELETE")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Client_Content, Manage_License_Content, Manage_Content")]
		ContentRatingResponse ResetBinaryRating(string bucketIdOrSlug, string idOrSlug, Guid? licenseId);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Binaries/{idOrSlug}/ViewCount", Method = "DELETE")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Client_Content, Manage_License_Content, Manage_Content")]
		ContentRatingResponse ResetBinaryViewCount(string bucketIdOrSlug, string idOrSlug, Guid? licenseId);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Binaries/{idOrSlug}/Rating", Method = "POST")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
		ContentRatingResponse AddBinaryRating(string bucketIdOrSlug, string idOrSlug, RatingRequest theRequest);
*/

		#endregion

		#region Images

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Images", Method = "POST")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content, Manage_Client_Content, Manage_License_Content")]
		[Alternative(ContentType = "text/html")]
		ImageDetailResponse CreateImage(string bucketIdOrSlug, StreamRequest body, string licenseId = null);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Images/{imageIdOrSlug}", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", Extensions = "jpg,jpeg,tiff,bmp,png,gif")]
		StreamResponse GetImage(string bucketIdOrSlug, string imageIdOrSlug, bool draft);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Images/Details/{imageIdOrSlug}", Method = "PUT")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content, Manage_Client_Content, Manage_License_Content")]
		ImageDetailResponse UpdateImageDetails(string bucketIdOrSlug, string imageIdOrSlug, ImageDetailRequest request);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Images/Details/{imageIdOrSlug}", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
		ImageDetailResponse GetImageDetails(string bucketIdOrSlug, string imageIdOrSlug, GetContentOptions options);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Images/Details/Slugs", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		SlugResponse GetImageSlug(string bucketIdOrSlug, string slug, string title);

		// was ImageDetailResponse
		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Images/{imageIdOrSlug}", Method = "DELETE")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content, Manage_Client_Content, Manage_License_Content")]
		ContentResponse DeleteImage(string bucketIdOrSlug, string imageIdOrSlug, Guid? licenseId, bool? publishedOnly = null);

/*
		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Images/{idOrSlug}/Rating", Method = "DELETE")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Client_Content, Manage_License_Content, Manage_Content")]
		ContentRatingResponse ResetImageRating(string bucketIdOrSlug, string idOrSlug, Guid? licenseId);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Images/{idOrSlug}/ViewCount", Method = "DELETE")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Client_Content, Manage_License_Content, Manage_Content")]
		ContentRatingResponse ResetImageViewCount(string bucketIdOrSlug, string idOrSlug, Guid? licenseId);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Images/{idOrSlug}/Rating", Method = "POST")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
		ContentRatingResponse AddImageRating(string bucketIdOrSlug, string idOrSlug, RatingRequest theRequest);
*/

		#endregion

		#region Modified Content

		[WebInvoke(UriTemplate = "Modifications", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
		ModifiedContentResponse GetContentModifications(DateTime startTime);

		[WebInvoke(UriTemplate = "Modifications/{licenseId}", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Client_Content")]
		ModifiedContentResponse GetContentModifications(Guid licenseId, DateTime startTime);

		#endregion
	}
}
