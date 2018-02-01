using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using StayWell.Interface;
using StayWell.ServiceDefinitions.Content.Objects;

namespace StayWell.ServiceDefinitions.Content
{
	[ServiceContract(Name = "StreamingMedia", Namespace = "http://www.kramesstaywell.com")]
	public interface IPublicProviderStreamingMediaService
	{
        [WebInvoke(UriTemplate = "{bucketIdOrSlug}/{idOrSlug}/Sources", Method = "GET")]
        [Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
        List<ProviderStreamingMediaFormatResponse> GetBrightcoveStreamingMediaSources(string bucketIdOrSlug, string idOrSlug, GetStreamingMediaOptions options);
	}
}
