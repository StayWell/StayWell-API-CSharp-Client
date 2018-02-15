using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using StayWell.Interface;
using StayWell.ServiceDefinitions.Content.Objects;

namespace StayWell.ServiceDefinitions.Content
{
    [ServiceContract(Name = "Content", Namespace = "http://www.kramesstaywell.com")]
	[Document]
    // ReSharper disable once SeviceContractWithoutOperations
    public interface IPublicContentService
	{
		#region Articles

		// this isn't very rest-y
		[WebInvoke(UriTemplate = "RelatedContent", Method = "GET")]
		[Document("Search related content", "Search for content with specific MeSH codes or MeSH codes matching a specific known content item")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		RelatedContentList SearchRelatedContent(RelatedContentSearchRequest request);

		[WebInvoke(UriTemplate = "", Method = "GET")]
        [Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		[Document("Search for content")]
        ContentList SearchContent(ContentSearchRequest request);

	[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{idOrSlug}", Method = "GET")]
        [Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		[Document("Get a specific piece of content")]
        ContentArticleResponse GetContent(string bucketIdOrSlug, string idOrSlug, GetContentOptions request);
     
		[WebInvoke(UriTemplate = "{bucketIdOrSlug}", Method = "POST")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content, Manage_Client_Content, Manage_License_Content")]
		[Document("Create an article or plain-text content item")]
		ContentArticleResponse CreateContent(string bucketIdOrSlug, NewContentRequest content);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{idOrSlug}", Method = "PUT")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content, Manage_Client_Content, Manage_License_Content")]
		[Document("Update and article or plain-text content item")]
		ContentArticleUpdateResponse UpdateContent(string bucketIdOrSlug, string idOrSlug, ContentArticleRequest content);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{idOrSlug}", Method = "DELETE")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content, Manage_Client_Content, Manage_License_Content")]
		[Document("Delete a specific piece of content")]
		ContentResponse DeleteContent(string bucketIdOrSlug, string idOrSlug, Guid? licenseId, bool? publishedOnly = null);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Slugs", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp, Logging = AllowedLogging.NeverLog)]
		[Document("Get a valid slug in a specific bucket using a content title or a potential slug")]
		SlugResponse GetSlug(string bucketIdOrSlug, string title, string slug);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{idOrSlug}/Collections", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		[Document("Get the collections associated with a piece of content")]
		AbbreviatedCollectionListResponse GetCollectionsForContent(string bucketIdOrSlug, string idOrSlug);

        [WebInvoke(UriTemplate = "Legacy/{bucketLegacyId}/{legacyId}", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		[Document("Get an article or plain-text piece of content by it's legacy (UCR) ID")]
		ContentArticleResponse GetLegacyContent(string bucketLegacyId, string legacyId, LegacyContentOptions options);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{idOrSlug}/Rating", Method = "DELETE")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Client_Content, Manage_License_Content, Manage_Content")]
		[Document("Reset rating counts for a piece of content")]
		ContentRatingResponse ResetRating(string bucketIdOrSlug, string idOrSlug, Guid? licenseId);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{idOrSlug}/ViewCount", Method = "DELETE")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Client_Content, Manage_License_Content, Manage_Content")]
		[Document("Reset view counts for a piece of content")]
		ContentRatingResponse ResetViewCount(string bucketIdOrSlug, string idOrSlug, Guid? licenseId);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{idOrSlug}/Rating", Method = "POST")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
		[Document("Add a rating for a piece of content")]
		ContentRatingResponse AddRating(string bucketIdOrSlug, string idOrSlug, RatingRequest theRequest);

		#endregion

		#region Images

		// publicly accessible image using license id
		[WebInvoke(UriTemplate = "{licenseId}/{bucketIdOrSlug}/Images/{imageIdOrSlug}", Method = "GET")]
		[Allow(ClientType = ClientType.Public, Extensions = "jpg,jpeg,tiff,bmp,png,gif")]
		[Document("Get an image using a specific license, without requiring authentication")]
		StreamResponse GetLicenseImage(string licenseId, string bucketIdOrSlug, string imageIdOrSlug);

		// publicly accessible image using application id
		[WebInvoke(UriTemplate = "ByApplication/{applicationId}/{bucketIdOrSlug}/Images/{imageIdOrSlug}", Method = "GET")]
		[Allow(ClientType = ClientType.Public, Extensions = "jpg,jpeg,tiff,bmp,png,gif")]
		[Document("Get an application using a specific application without requiring authentication")]
		StreamResponse GetApplicationImage(string applicationId, string bucketIdOrSlug, string imageIdOrSlug);

		#endregion

		#region History

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{contentIdOrSlug}/History", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
		[Document("Get the history of a piece of content")]
		ContentHistoryResponse GetHistory(string bucketIdOrSlug, string contentIdOrSlug, Guid? licenseId);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{contentIdOrSlug}/History/{historyId}", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
		[Document("Get a previous version of a specific article or plain-text piece of content")]
		ContentArticleResponse GetHistoryContent(string bucketIdOrSlug, string contentIdOrSlug, string historyId, Guid? licenseId, bool? includeBody);

		#endregion

		#region Taxonomies

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{contentIdOrSlug}/Taxonomies", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content")]
		[Document("Get the taxonomies for a specific piece of content")]
		TaxonomyListResponse SearchTaxonomies(string bucketIdOrSlug, string contentIdOrSlug);

		#endregion

		#region Binaries

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Binaries", Method = "POST")]
		[Alternative(ContentType = "text/html")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content, Manage_Client_Content, Manage_License_Content")]
		[Document("Create the metadata for a binary resource")]
		BinaryDetailResponse CreateBinary(string bucketIdOrSlug, StreamRequest body, string licenseId = null);

		// publicly accessible binary using license id
		[WebInvoke(UriTemplate = "{licenseId}/{bucketIdOrSlug}/Binaries/{binaryIdOrSlug}", Method = "GET")]
		[Allow(ClientType = ClientType.Public, Extensions = "jpg,jpeg,tiff,bmp,png,gif")]
		[Document("Get a binary resource for a specific license without requiring authentication")]
		StreamResponse GetLicenseBinary(string licenseId, string bucketIdOrSlug, string binaryIdOrSlug);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Binaries/{binaryIdOrSlug}", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", Extensions = "jpg,jpeg,tiff,bmp,png,gif")]
		[Document("Get a specific binary resource")]
		StreamResponse GetBinary(string bucketIdOrSlug, string binaryIdOrSlug);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Binaries/Details/{binaryIdOrSlug}", Method = "PUT")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content, Manage_Client_Content, Manage_License_Content")]
		[Document("Update the metadata for a binary resource")]
		BinaryDetailResponse UpdateBinaryDetails(string bucketIdOrSlug, string binaryIdOrSlug, BinaryDetailRequest request);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Binaries/Details/{binaryIdOrSlug}", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
		[Document("Get a binary resource")]
		BinaryDetailResponse GetBinaryDetails(string bucketIdOrSlug, string binaryIdOrSlug, GetContentOptions options);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Binaries/Details/Slugs", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		[Document("Get a valid slug for a binary resource")]
		SlugResponse GetBinarySlug(string bucketIdOrSlug, string slug, string title);

		// prev. returned BinaryDetailResponse <== return specific types for image, binary, streaming media?
		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Binaries/{binaryIdOrSlug}", Method = "DELETE")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content, Manage_Client_Content, Manage_License_Content")]
		[Document("Delete a binary resource")]
		ContentResponse DeleteBinary(string bucketIdOrSlug, string binaryIdOrSlug, Guid? licenseId, bool? publishedOnly = null);

		#endregion

		#region Images

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Images", Method = "POST")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content, Manage_Client_Content, Manage_License_Content")]
		[Alternative(ContentType = "text/html")]
		[Document("Create image")]
		ImageDetailResponse CreateImage(string bucketIdOrSlug, StreamRequest body, string licenseId = null);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Images/{imageIdOrSlug}", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", Extensions = "jpg,jpeg,tiff,bmp,png,gif")]
		[Document("Get an image")]
		StreamResponse GetImage(string bucketIdOrSlug, string imageIdOrSlug, bool draft);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Images/Details/{imageIdOrSlug}", Method = "PUT")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content, Manage_Client_Content, Manage_License_Content")]
		[Document("Update the metadata for an image")]
		ImageDetailResponse UpdateImageDetails(string bucketIdOrSlug, string imageIdOrSlug, ImageDetailRequest request);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Images/Details/{imageIdOrSlug}", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
		[Document("Get the metadata for an image")]
		ImageDetailResponse GetImageDetails(string bucketIdOrSlug, string imageIdOrSlug, GetContentOptions options);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Images/Details/Slugs", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		[Document("Get a valid slug for an image")]
		SlugResponse GetImageSlug(string bucketIdOrSlug, string slug, string title);

		// was ImageDetailResponse
		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/Images/{imageIdOrSlug}", Method = "DELETE")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content, Manage_Client_Content, Manage_License_Content")]
		[Document("Delete an image")]
		ContentResponse DeleteImage(string bucketIdOrSlug, string imageIdOrSlug, Guid? licenseId, bool? publishedOnly = null);

		#endregion

		#region Modified Content

		[WebInvoke(UriTemplate = "Modifications", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
		[Document("Get content modified after a specific date")]
		ModifiedContentResponse GetContentModifications(DateTime startTime, bool includeBlockedContent = false);

		[WebInvoke(UriTemplate = "Modifications/{licenseId}", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Client_Content")]
		[Document("Get content modifications for a specific license")]
		ModifiedContentResponse GetContentModifications(Guid licenseId, DateTime startTime, bool includeBlockedContent = false);

        #endregion


		[WebInvoke(UriTemplate = "SearchSuggestions", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
		SearchSuggestionsResponse GetContentSearchSuggestions(SearchSuggestionsRequest request);

		// TODO: add spellcheck endpoint
		/*
		[WebInvoke(UriTemplate = "/SpellCheck", Method = "POST")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
		ResultList<string> CheckSpelling(string[] str);
		*/
    }

	
}
