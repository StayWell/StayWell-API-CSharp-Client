using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using StayWell.Interface;
using StayWell.ServiceDefinitions.Content.Objects;
using StayWell.ServiceDefinitions.Mapping.Objects;

namespace StayWell.ServiceDefinitions.Mapping
{
	[ServiceContract(Name = "Mapping", Namespace = "http://www.kramesstaywell.com")]
	public interface IPublicMappingService
	{
		[WebInvoke(UriTemplate = "/MappingModule/Statistics", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Mapping")]
		MappingStatisticsResponse GetStatistics();

		[WebInvoke(UriTemplate = "/MappingModule/{licenseId}/Statistics", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Mapping")]
		MappingStatisticsResponse GetStatistics(Guid licenseId);

		[WebInvoke(UriTemplate = "/MappingModule/{licenseId}/{mappingType}/Mappings", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Mapping")]
		PagedMappingResponseList SearchMappings(Guid licenseId, string mappingType, MappingSearchRequest request);

		[WebInvoke(UriTemplate = "/MappingModule/{licenseId}/{mappingType}/Mappings", Method = "POST")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Mapping")]
		MappingResponse CreateMapping(Guid licenseId, string mappingType, MappingRequest request);

		[WebInvoke(UriTemplate = "/MappingModule/{licenseId}/{mappingType}/Mappings/{id}", Method = "DELETE")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Mapping")]
		MappingResponse DeleteMapping(Guid licenseId, string mappingType, Guid id);

		[WebInvoke(UriTemplate = "/MappingModule/{licenseId}/{mappingType}/Mappings/{id}", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Mapping")]
		MappingResponse GetMapping(Guid licenseId, string mappingType, Guid id);

		[WebInvoke(UriTemplate = "/MappingModule/{licenseId}/{mappingType}/Mappings/{id}", Method = "PUT")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Mapping")]
		MappingResponse UpdateMapping(Guid licenseId, string mappingType, Guid id, MappingRequest request);

		[WebInvoke(UriTemplate = "/MappingModule/{licenseId}/{mappingType}/Mappings/{id}/Published", Method = "PUT")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Mapping")]
		MappingResponse PublishMapping(Guid licenseId, string mappingType, Guid id);

		[WebInvoke(UriTemplate = "/MappingModule/{licenseId}/{mappingType}/Mappings/{id}/ServiceLines", Method = "DELETE")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Mapping")]
		ServiceLineResponse RemoveServiceLineMetadata(Guid licenseId, string mappingType, Guid id, ServiceLineRequest request);

		[WebInvoke(UriTemplate = "/MappingModule/{licenseId}/{mappingType}/Mappings/{id}/ServiceLines", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Mapping")]
		ServiceLineResponseList GetServiceLineMetadata(Guid licenseId, string mappingType, Guid id, bool includeUnpublished = false);

		[WebInvoke(UriTemplate = "/MappingModule/{licenseId}/{mappingType}/Mappings/{id}/ServiceLines", Method = "POST")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Mapping")]
		ServiceLineResponse MapServiceLineMetadata(Guid licenseId, string mappingType, Guid id, ServiceLineRequest request);

		[WebInvoke(UriTemplate = "/MappingModule/{licenseId}/{mappingType}/Mappings/{id}/ServiceLines", Method = "PUT")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Mapping")]
		ServiceLineRequestList UpdateServiceLineMetadata(Guid licenseId, string mappingType, Guid id, ServiceLineRequestList request);

		[WebInvoke(UriTemplate = "/MappingModule/{licenseId}/{mappingType}/Mappings/{id}/Unpublished", Method = "PUT")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Mapping")]
		MappingResponse UnpublishMapping(Guid licenseId, string mappingType, Guid id);

		[WebInvoke(UriTemplate = "/MappingModule/{licenseId}/{mappingType}/Mappings/All/Published", Method = "PUT")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Mapping")]
		PublishedChangeResponse PublishAllMappings(Guid licenseId, string mappingType);

		[WebInvoke(UriTemplate = "/MappingModule/{licenseId}/{mappingType}/Mappings/All/Unpublished", Method = "PUT")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Manage_Mapping")]
        UnpublishedChangeResponse UnpublishAllMappings(Guid licenseId, string mappingType);

        [WebInvoke(UriTemplate = "/MappingModule/{licenseId}/{mappingType}/Mappings/File", Method = "POST")]
        [OperationContract]
        [Allow(ClientType = ClientType.Internal, Rights = "Manage_Mapping")]
        UploadResponse UploadMappings(Guid licenseId, string mappingType, StreamRequest request);

	    [WebInvoke(UriTemplate = "/MappingModule/{licenseId}/{mappingType}/Mappings/FileUpdate/{id}", Method = "PUT")]
	    [OperationContract]
	    [Allow(ClientType = ClientType.Internal, Rights = "Manage_Mapping")]
	    UploadResponse UpdateAndUploadMappings(Guid licenseId, string mappingType, Guid id, StreamRequest request);

		[WebInvoke(UriTemplate = "/MappingModule/{licenseId}/{mappingType}/ServiceLines/{audienceSlug}/{serviceLineSlug}/{pageKeywordSlug}/Mappings", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Mapping", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		ServiceLineMappingResponseList GetServiceLineMappings(Guid licenseId, string mappingType, string audienceSlug, string serviceLineSlug, string pageKeywordSlug, bool includeUnpublished = false);

		[WebInvoke(UriTemplate = "/MappingModule/{licenseId}/{mappingType}/ServiceLines/{audienceSlug}/{serviceLineSlug}/Mappings", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Mapping", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		ServiceLineMappingResponseList GetServiceLineMappings(Guid licenseId, string mappingType, string audienceSlug, string serviceLineSlug, bool includeUnpublished = false);

		[WebInvoke(UriTemplate = "/MappingModule/{mappingType}/ServiceLines/{audienceSlug}/{serviceLineSlug}/{pageKeywordSlug}/Mappings", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Mapping", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		ServiceLineMappingResponseList GetServiceLineMappings(string mappingType, string audienceSlug, string serviceLineSlug, string pageKeywordSlug, bool includeUnpublished = false);

		[WebInvoke(UriTemplate = "/MappingModule/{mappingType}/ServiceLines/{audienceSlug}/{serviceLineSlug}/Mappings", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Mapping", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		ServiceLineMappingResponseList GetServiceLineMappings(string mappingType, string audienceSlug, string serviceLineSlug, bool includeUnpublished = false);

		[WebInvoke(UriTemplate = "/Reports/Mapping/ServiceLineReport", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Mapping")]
		MappingServiceLineReport GetReport(MappingServiceLineReportRequest request);

		[WebInvoke(UriTemplate = "/Reports/Mapping/ServiceLineReportExport.xlsx", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Mapping")]
		StreamResponse GetReportExport(MappingServiceLineExportRequest request);

		[WebInvoke(UriTemplate = "/MappingModule/Types", Method = "GET")]
		[OperationContract]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Mapping")]
		MappingTypeListResponse GetTypes();

	}
}
