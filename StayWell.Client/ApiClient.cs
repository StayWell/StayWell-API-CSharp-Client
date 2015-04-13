using StayWell.ClientFramework;
using StayWell.ClientFramework.Enums;
using StayWell.ClientFramework.Interfaces;
using StayWell.ServiceDefinitions.Authorization;
using StayWell.ServiceDefinitions.Buckets;
using StayWell.ServiceDefinitions.Collections;
using StayWell.ServiceDefinitions.Content;
using StayWell.ServiceDefinitions.Languages;
using StayWell.ServiceDefinitions.Mapping;
using StayWell.ServiceDefinitions.Security;
using StayWell.ServiceDefinitions.ServiceLines;
using StayWell.ServiceDefinitions.Taxonomies;

namespace StayWell.Client
{
	public class ApiClient : ServiceClient
	{
		public ApiClient(string applicationId, string applicationSecret)
			: base(applicationId, applicationSecret)
		{
		}

		public ApiClient(string applicationId, string applicationSecret, TokenStoreType tokenStoreType)
			: base(applicationId, applicationSecret, tokenStoreType)
		{
		}

		public ApiClient(string applicationId, string applicationSecret, ITokenStore tokenStore)
			: base(applicationId, applicationSecret, tokenStore)
		{
		}

		public ApiClient(string serviceUri, string applicationId, string applicationSecret, TokenStoreType tokenStoreType)
			: base(serviceUri, applicationId, applicationSecret, tokenStoreType)
		{
		}

		public ApiClient(string serviceUri, string applicationId, string applicationSecret, ITokenStore tokenStore)
			: base(serviceUri, applicationId, applicationSecret, tokenStore)
		{
		}

		public IOAuthService OAuth { get { return GetService<IOAuthService>(); } }
		public IPublicBucketService Buckets { get { return GetService<IPublicBucketService>(); } }
		public IPublicCollectionService Collections { get { return GetService<IPublicCollectionService>(); } }
		public IPublicContentService Content { get { return GetService<IPublicContentService>(); } }
		public IPublicStreamingMediaService StreamingMedia { get { return GetService<IPublicStreamingMediaService>(); } }
		public IPublicLanguageService Languages { get { return GetService<IPublicLanguageService>(); } }
		public IPublicMappingService Mappings { get { return GetService<IPublicMappingService>(); } }
		public IPublicSecurityService Security { get { return GetService<IPublicSecurityService>(); } }
		public IPublicServiceLineService ServiceLines { get { return GetService<IPublicServiceLineService>(); } }
		public IPublicTaxonomyService Taxonomies { get { return GetService<IPublicTaxonomyService>(); } }
	}
}
