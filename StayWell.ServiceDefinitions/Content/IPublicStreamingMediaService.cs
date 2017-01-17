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
		StreamingMediaResponse GetStreamingMedia(string bucketIdOrSlug, string idOrSlug, GetContentOptions options);

		// publicly accessible using license id
		[WebInvoke(UriTemplate = "Captions/{licenseId}/{bucketIdOrSlug}/{idOrSlug}", Method = "GET")]
		[Allow(ClientType = ClientType.Public, ReturnModifier = OperationReturnModifier.XmlOnlyPassThrough)]
		string GetClosedCaptioning(string licenseId, string bucketIdOrSlug, string idOrSlug);
        
		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{idOrSlug}/Rating", Method = "POST")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
		ContentRatingResponse AddRating(string bucketIdOrSlug, string idOrSlug, RatingRequest theRequest);

		[WebInvoke(UriTemplate = "MediaProviders", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
		MediaProviderResponseList GetProviders();

		[WebInvoke(UriTemplate = "FileFormats", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
		FileFormatResponseList GetFileFormats();
	}
}
