using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;
using StayWell.ClientFramework.Enums;
using StayWell.ClientFramework.Interfaces;
using StayWell.ClientFramework.Objects;
using StayWell.Interface;
using StayWell.ServiceDefinitions.Authorization.Objects;
using IServiceChannel = StayWell.ClientFramework.Interfaces.IServiceChannel;

namespace StayWell.ClientFramework.Internal
{
	internal class ServiceProxy<TService> : RealProxy
	{
		private readonly ITokenStore _tokenStore;
		private readonly IServiceChannel _serviceChannel;
		private readonly string _serviceUri;
		private readonly string _applicationId;
		private readonly string _applicationSecret;

		public ServiceProxy(string serviceUri)
			: this(serviceUri, null, null, null)
		{

		}

		public ServiceProxy(string serviceUri, string applicationId, string applicationSecret, ITokenStore tokenStore)
			: base(typeof(TService))
		{
			_serviceUri = serviceUri;
			_applicationId = applicationId;
			_applicationSecret = applicationSecret;
			_serviceChannel = new ServiceChannel(serviceUri);
			_tokenStore = tokenStore;
		}

		public TService GetService()
		{
			return (TService)GetTransparentProxy();
		}

		private IMessage InvokeRecursive(IMessage msg, bool allowRetry, AccessToken token)
		{
			IMethodCallMessage methodCallMessage = (IMethodCallMessage)msg;

			MethodInfo methodInfo = methodCallMessage.MethodBase as MethodInfo;

			if (methodInfo == null)
				throw new InvalidOperationException("The service method signature is invalid.");

			object[] methodAttributes = methodInfo.GetCustomAttributes(false);

			HttpMethod method = GetMethod(methodAttributes);

			NameValueCollection parameters = GetParameters(methodInfo, methodCallMessage, method);

			object body = GetBody(method, methodCallMessage);

			AllowAttribute allowAttribute = (AllowAttribute)methodAttributes.SingleOrDefault(item => item is AllowAttribute);

			NameValueCollection headers = GetHeaders(allowAttribute, token);

			string moduleName = GetModuleName(methodInfo);

			string operationName = GetOperationName(methodInfo, methodAttributes, parameters);

			if (allowAttribute != null)
			{
				switch (allowAttribute.ReturnModifier)
				{
					case OperationReturnModifier.XmlOnlyPassThrough:
						operationName += ".xml";
						break;
				}
			}

			OperationRequest operation = new OperationRequest
			{
				OperationType = methodInfo.ReturnType == typeof(OAuthTokenResponse) ? OperationType.OAuth : OperationType.KswApi,
				HttpMethod = method,
				Body = body,
				Headers = headers,
				ModuleName = moduleName,
				OperationName = operationName,
				QueryParameters = parameters,
				ResultType = methodInfo.ReturnType
			};

			try
			{
				object response = _serviceChannel.Invoke(operation);

				return new ReturnMessage(response, null, 0, null, methodCallMessage);
			}
			catch (ServiceException exception)
			{
				if (exception.StatusCode != HttpStatusCode.Unauthorized)
					return new ReturnMessage(exception, methodCallMessage);

				if (_tokenStore == null)
					return new ReturnMessage(exception, methodCallMessage);

				_tokenStore.RemoveToken(_applicationId);

				if (!allowRetry)
					return new ReturnMessage(exception, methodCallMessage);

				return RefreshTokenAndRetryOperation(exception, methodCallMessage, allowAttribute, token);
			}
		}

		private IMessage RefreshTokenAndRetryOperation(ServiceException exception, IMethodCallMessage methodCallMessage, AllowAttribute allowAttribute, AccessToken token)
		{
			// try refreshing the token
			if (token != null && token.Type == AccessTokenType.Application)
			{
				AccessToken newToken = GetNewAccessToken();

				if (newToken == null)
				{
					// this call is only made after we're sure that a token store exists,
					// we can safely assume it is not null here
					_tokenStore.RemoveToken(_applicationId);

					return new ReturnMessage(exception, methodCallMessage);
				}

				return InvokeRecursive(methodCallMessage, false, newToken);
			}
			else
			{
				if (token == null || string.IsNullOrEmpty(token.RefreshToken))
					return new ReturnMessage(exception, methodCallMessage);

				AccessToken newToken = RefreshAccessToken(token);

				if (newToken == null)
					return new ReturnMessage(exception, methodCallMessage);

				return InvokeRecursive(methodCallMessage, false, newToken);
			}
		}

		private AccessToken GetNewAccessToken()
		{
			if (_tokenStore == null)
				return null;

			if (_applicationId == null || _applicationSecret == null)
				return null;

			OAuthClient oauthClient = new OAuthClient(_serviceUri);

			AccessToken token = oauthClient.GetApplicationToken(_applicationId, _applicationSecret);

			_tokenStore.SetToken(token, _applicationId);

			return token;
		}

		private AccessToken RefreshAccessToken(AccessToken token)
		{
			if (_applicationId == null || _applicationSecret == null)
				return null;

			OAuthClient oauthClient = new OAuthClient(_serviceUri);

			AccessToken newToken;

			try
			{
				newToken = oauthClient.RefreshUserToken(token.RefreshToken, _applicationId, _applicationSecret);
			}
			catch (OAuthException)
			{
				_tokenStore.RemoveToken(_applicationId);
				return null;
			}

			_tokenStore.SetToken(newToken, _applicationId);

			return newToken;
		}

		private object GetBody(HttpMethod method, IMethodCallMessage message)
		{
			if (method == HttpMethod.Get || method == HttpMethod.Delete)
				return null;

			foreach (object body in message.Args)
			{
				if (body == null)
					continue;

				Type type = body.GetType();
				if (type != typeof(string) && type.IsClass)
					return body;
			}

			return null;
		}

		public override IMessage Invoke(IMessage msg)
		{
			try
			{
				AccessToken token = null;
				if (_tokenStore != null)
					token = _tokenStore.GetToken(_applicationId);

				return InvokeRecursive(msg, true, token);
			}
			catch (Exception exception)
			{
				return new ReturnMessage(exception, (IMethodCallMessage)msg);
			}
		}

		protected virtual string GetClientAccessToken()
		{
			throw new InvalidOperationException("The current method does not support client access authorization");
		}

		private string GetOperationName(MethodInfo methodInfo, object[] attributes, NameValueCollection parameters)
		{
			string method = GetMethodNameFromAttributes(methodInfo, attributes, parameters);

			if (method != null)
				return method;

			return methodInfo.Name;
		}

		private string GetMethodNameFromAttributes(MethodInfo methodInfo, object[] attributes, NameValueCollection parameters)
		{
			WebGetAttribute webGetAttribute = attributes.FirstOrDefault(item => item is WebGetAttribute) as WebGetAttribute;
			if (webGetAttribute != null)
				return GetMethodNameFromUriTemplate(webGetAttribute.UriTemplate, methodInfo, parameters);

			WebInvokeAttribute webInvokeAttribute = attributes.FirstOrDefault(item => item is WebInvokeAttribute) as WebInvokeAttribute;
			if (webInvokeAttribute != null)
				return GetMethodNameFromUriTemplate(webInvokeAttribute.UriTemplate, methodInfo, parameters);

			return null;
		}

		private string GetMethodNameFromUriTemplate(string uriTemplate, MethodInfo methodInfo, NameValueCollection parameters)
		{
			if (uriTemplate == null)
				return null;

			ParameterInfo[] parameterInfos = methodInfo.GetParameters();

			foreach (ParameterInfo parameterInfo in parameterInfos)
			{
				string name = parameterInfo.Name;
				string value = parameters[name];
				string inlineParameter = '{' + name + '}';

				if (uriTemplate.Contains(inlineParameter))
				{
					uriTemplate = uriTemplate.Replace(inlineParameter, Uri.EscapeDataString(value ?? string.Empty));
					parameters.Remove(name);
				}
			}

			return uriTemplate;
		}

		private NameValueCollection GetParameters(MethodInfo methodInfo, IMethodCallMessage message, HttpMethod verb)
		{
			ParameterInfo[] parameterInfos = methodInfo.GetParameters();

			if (parameterInfos.Length != message.Args.Length)
				throw new InvalidOperationException("Number of parameters does not match the method signature.");

			if (parameterInfos.Length == 0)
				return null;

			NameValueCollection parameters = new NameValueCollection(message.Args.Length);

			for (int i = 0; i < parameterInfos.Length; i++)
			{
				object current = message.Args[i];

				if (current == null)
					continue;

				ParameterInfo parameterInfo = parameterInfos[i];
				Type type = parameterInfo.ParameterType;
				if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
					type = type.GetGenericArguments()[0];
				if (type.IsPrimitive || type == typeof(string) || type == typeof(Guid) || type.IsEnum || type.IsArray)
				{
					string name = parameterInfo.Name;

					MessageParameterAttribute attribute = (MessageParameterAttribute)Attribute.GetCustomAttribute(parameterInfo, typeof(MessageParameterAttribute));

					if (attribute != null && !string.IsNullOrEmpty(attribute.Name))
						name = attribute.Name;

					string value = !parameterInfo.ParameterType.IsArray ?
						current.ToString() :
						string.Join(",", ((IEnumerable)current).Cast<object>().ToArray());

					parameters.Add(name, value);
				}
				else if (type.IsClass && (verb == HttpMethod.Get || verb == HttpMethod.Delete))
				{
					foreach (PropertyInfo propertyInfo in parameterInfo.ParameterType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
					{
						if (!propertyInfo.CanRead || !propertyInfo.CanWrite)
							continue;

						Type propertyType = propertyInfo.PropertyType;

						if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
							propertyType = propertyType.GetGenericArguments()[0];

						object objectValue = propertyInfo.GetValue(current, null);

						if (objectValue == null)
							continue;

						if (propertyType.IsPrimitive || propertyType == typeof(string) || propertyType == typeof(Guid) || propertyType.IsEnum || propertyType.IsArray)
						{
							DataMemberAttribute attribute = (DataMemberAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(DataMemberAttribute));
							string name = attribute != null && !string.IsNullOrEmpty(attribute.Name)
											  ? attribute.Name
											  : propertyInfo.Name;

							string value = !propertyType.IsArray
											   ? objectValue.ToString()
											   : string.Join(",", ((IEnumerable)objectValue).Cast<object>());

							parameters.Add(name, value);
						}
						else if (propertyType.IsClass && typeof(IList).IsAssignableFrom(propertyType))
						{
							DataMemberAttribute attribute = (DataMemberAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(DataMemberAttribute));
							string name = attribute != null && !string.IsNullOrEmpty(attribute.Name)
											  ? attribute.Name
											  : propertyInfo.Name;

							IList list = (IList)objectValue;

							string value = string.Join(",", list.Cast<object>());

							parameters.Add(name, value);
						}
					}
				}
			}

			return parameters;
		}

		private NameValueCollection GetHeaders(AllowAttribute allowAttribute, AccessToken token)
		{
			if (allowAttribute == null)
				return null;

			switch (allowAttribute.ClientType)
			{
				// this was changed: send access token even on public calls
				// (this allows tracking of test requests in the logs)
				case ClientType.Public:
				case ClientType.Authentication:
				case ClientType.Internal:
				case ClientType.Any:
					return GetAuthorizationHeaders(token);
				case ClientType.External:
					throw new NotImplementedException();
				default:
					throw new InvalidOperationException("The AllowAttribute's client type is invalid.");
			}
		}

		private NameValueCollection GetAuthorizationHeaders(AccessToken token)
		{
			if (token == null)
			{
				token = GetNewAccessToken();

				if (token == null)
					return null;
			}

			string headerValue = "Bearer " + token.Token;

			return new NameValueCollection
			       	{
			       		{"Authorization", headerValue}
			       	};
		}

		private HttpMethod GetMethod(object[] attributes)
		{
			if (attributes.Any(item => item is WebGetAttribute))
				return HttpMethod.Get;

			WebInvokeAttribute invoke = attributes.FirstOrDefault(item => item is WebInvokeAttribute) as WebInvokeAttribute;

			if (invoke == null)
				return HttpMethod.Get;

			if (string.IsNullOrEmpty(invoke.Method))
				return HttpMethod.Post;

			switch (invoke.Method)
			{
				case HttpMethodString.GET:
					return HttpMethod.Get;
				case HttpMethodString.POST:
					return HttpMethod.Post;
				case HttpMethodString.PUT:
					return HttpMethod.Put;
				case HttpMethodString.DELETE:
					return HttpMethod.Delete;
				default:
					throw new ArgumentException(string.Format("{0} is not a recognized HTTP method", invoke.Method));
			}
		}

		private string GetModuleName(MethodInfo methodInfo)
		{
			// use the interface that declares the method: it could be an inherited method
			if (methodInfo.DeclaringType == null)
				return null;

			object[] interfaceAttributes = methodInfo.DeclaringType.GetCustomAttributes(false);

			ServiceContractAttribute serviceContract = (ServiceContractAttribute)interfaceAttributes.FirstOrDefault(item => item is ServiceContractAttribute);

			if (serviceContract == null)
				throw new InvalidOperationException("The service interface does not have a service contract attribute");

			return serviceContract.Name;
		}
	}
}
