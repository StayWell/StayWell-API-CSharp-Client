using System;
using System.Collections.Concurrent;
using StayWell.ClientFramework.Enums;
using StayWell.ClientFramework.Interfaces;
using StayWell.ClientFramework.Internal;
using StayWell.ClientFramework.TokenStores;

namespace StayWell.ClientFramework
{
	public abstract class ServiceClient
	{
		public const string DEFAULT_API_URI = "https://api.kramesstaywell.com";

		#region Private Fields

		private readonly ConcurrentDictionary<Type, object> _services = new ConcurrentDictionary<Type, object>();
		private readonly ITokenStore _tokenStore;
		private readonly string _applicationId;
		private readonly string _applicationSecret;

		#endregion Private Fields

		#region Public Properties

		public string ServiceUri { get; private set; }

		#endregion Public Properties

		#region Public Constructors

		protected ServiceClient(ServiceClient existingClient)
		{
			_services = existingClient._services;
			ServiceUri = existingClient.ServiceUri;
			_applicationId = existingClient._applicationId;
			_applicationSecret = existingClient._applicationSecret;
			_tokenStore = existingClient._tokenStore;
		}

		protected ServiceClient(string applicationId, string applicationSecret)
			: this(DEFAULT_API_URI, applicationId, applicationSecret, TokenStoreType.PerClient)
		{
		}

		protected ServiceClient(string applicationId, string applicationSecret, TokenStoreType tokenStoreType)
			: this(DEFAULT_API_URI, applicationId, applicationSecret, tokenStoreType)
		{
		}

		protected ServiceClient(string applicationId, string applicationSecret, ITokenStore tokenStore)
			: this(DEFAULT_API_URI, applicationId, applicationSecret, tokenStore)
		{
		}

		protected ServiceClient(string serviceUri, string applicationId, string applicationSecret, TokenStoreType tokenStoreType)
		{
			ServiceUri = serviceUri;

			_applicationId = applicationId;
			_applicationSecret = applicationSecret;

			_tokenStore = GetTokenStore(tokenStoreType);
		}

		protected ServiceClient(string serviceUri, string applicationId, string applicationSecret, ITokenStore tokenStore)
		{
			ServiceUri = serviceUri;
			_applicationId = applicationId;
			_applicationSecret = applicationSecret;

			_tokenStore = tokenStore;
		}

		#endregion Public Constructors

		/// <summary>
		/// Lazy loads the service
		/// </summary>
		/// <typeparam name="TService"></typeparam>
		/// <returns></returns>
		public TService GetService<TService>()
		{
			return (TService)_services.GetOrAdd(typeof(TService), type => CreateService<TService>());
		}

		private TService CreateService<TService>()
		{
			ServiceProxy<TService> proxy = new ServiceProxy<TService>(ServiceUri, _applicationId, _applicationSecret, _tokenStore);

			return (TService)proxy.GetTransparentProxy();
		}

		private ITokenStore GetTokenStore(TokenStoreType tokenStoreType)
		{
			switch (tokenStoreType)
			{
				case TokenStoreType.Session:
					return new SessionTokenStore();
				case TokenStoreType.Cookie:
					return new CookieTokenStore();
				case TokenStoreType.SingleApplication:
					return new GlobalApplicationStore();
				case TokenStoreType.PerClient:
					return new DefaultTokenStore();
				default:
					throw new ArgumentOutOfRangeException("tokenStoreType");
			}
		}
	}
}
