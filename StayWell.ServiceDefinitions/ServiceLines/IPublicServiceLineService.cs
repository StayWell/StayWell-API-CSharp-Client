﻿using System.ServiceModel;
using System.ServiceModel.Web;
using StayWell.Interface;
using StayWell.ServiceDefinitions.Content.Objects;
using StayWell.ServiceDefinitions.ServiceLines.Objects;

namespace StayWell.ServiceDefinitions.ServiceLines
{
	[ServiceContract(Name = "ServiceLines", Namespace = "http://www.kramesstaywell.com")]
	public interface IPublicServiceLineService
	{
		[WebInvoke(UriTemplate = "", Method = "GET")]
		[Allow(ClientType = ClientType.Public, SpecialAccess = AllowedSpecialAccess.Jsonp)]
		[Document("Search service lines")]
		PagedServiceLineResponseList SearchServiceLines(PagedServiceLineSearchRequest request);

		[WebInvoke(UriTemplate = "Hierarchy", Method = "GET")]
		[Allow(ClientType = ClientType.Public, SpecialAccess = AllowedSpecialAccess.Jsonp)]
		[Document("Search service lines and return the results as a hierarchy")]
		ServiceLineHierarchy SearchServiceLines(ServiceLineSearchRequest request);

		[WebInvoke(UriTemplate = "Audiences", Method = "GET")]
		[Allow(ClientType = ClientType.Public, SpecialAccess = AllowedSpecialAccess.Jsonp)]
		[Document("Get all service line audiences")]
		ServiceLineResponseList GetAllAudiences();

		[WebInvoke(UriTemplate = "Audiences/{audienceSlug}", Method = "GET")]
		[Allow(ClientType = ClientType.Public, SpecialAccess = AllowedSpecialAccess.Jsonp)]
		[Document("Get all service lines in a specific audience")]
		ServiceLineResponseList GetServiceLinesInAudience(string audienceSlug);

		[WebInvoke(UriTemplate = "Audiences/{audienceSlug}/{serviceLineSlug}", Method = "GET")]
		[Allow(ClientType = ClientType.Public, SpecialAccess = AllowedSpecialAccess.Jsonp)]
		[Document("Get all keywords in a specific service line")]
		ServiceLineResponseList GetKeywordsInServiceLine(string audienceSlug, string serviceLineSlug);

		[WebInvoke(UriTemplate = "Audiences/{audienceSlug}", Method = "POST")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content")]
		ServiceLineItemResponse CreateServiceLine(string audienceSlug, CreateServiceLineRequest request);

		[WebInvoke(UriTemplate = "Audiences/{audienceSlug}/{serviceLineSlug}", Method = "PUT")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content")]
		ServiceLineItemResponse UpdateServiceLine(string audienceSlug, string serviceLineSlug, UpdateServiceLineRequest request);

		[WebInvoke(UriTemplate = "Audiences/{audienceSlug}/{serviceLineSlug}", Method = "DELETE")]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Content")]
		ServiceLineItemResponse DeleteServiceLine(string audienceSlug, string serviceLineSlug);
	}
}
