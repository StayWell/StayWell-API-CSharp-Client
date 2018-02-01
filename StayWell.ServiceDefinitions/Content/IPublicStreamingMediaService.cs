using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using StayWell.Interface;
using StayWell.ServiceDefinitions.Content.Objects;

namespace StayWell.ServiceDefinitions.Content
{
	[ServiceContract(Name = "StreamingMedia", Namespace = "http://www.kramesstaywell.com")]
	public interface IPublicStreamingMediaService
	{
		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{idOrSlug}", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
        [Document("Get the metadata for a streaming media resource")]
        StreamingMediaResponse GetStreamingMedia(string bucketIdOrSlug, string idOrSlug, GetStreamingMediaOptions options);

        [WebInvoke(UriTemplate = "Legacy/{bucketLegacyId}/{legacyId}", Method = "GET")]
        [Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
        [Document("Get the metadata for a streaming media resource by legacy id")]
        StreamingMediaResponse GetLegacyStreamingMedia(string bucketLegacyId, string legacyId, GetLegacyStreamingMediaOptions options);

		// publicly accessible using license id
		[WebInvoke(UriTemplate = "Captions/{licenseId}/{bucketIdOrSlug}/{idOrSlug}", Method = "GET")]
		[Allow(ClientType = ClientType.Public, ReturnModifier = OperationReturnModifier.XmlOnlyPassThrough)]
        [Document("Get closed captioning information for a streaming media resource")]
        string GetClosedCaptioning(Guid licenseId, string bucketIdOrSlug, string idOrSlug);
        
		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{idOrSlug}/Rating", Method = "POST")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
		[Document("Add a rating for a streaming media resource")]
		ContentRatingResponse AddRating(string bucketIdOrSlug, string idOrSlug, RatingRequest theRequest);

		[WebInvoke(UriTemplate = "MediaProviders", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
        [Document("Get streaming media providers")]
        MediaProviderResponseList GetProviders(Guid? licenseId);

		[WebInvoke(UriTemplate = "FileFormats", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
		[Document("Get file formats for streaming media resources")]
		FileFormatResponseList GetFileFormats();
	}
}
