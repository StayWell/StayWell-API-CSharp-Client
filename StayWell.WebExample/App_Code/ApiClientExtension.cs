using StayWell.ServiceDefinitions.Content.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StayWell.Client;
using StayWell.ServiceDefinitions.Mapping.Objects;
using StayWell.Interface;

namespace StayWell.WebExample.App_Code
{
    public class ApiClientExtension
    {

        private const int DEFAULT_COUNT = 50;
        private ApiClient _client;
        private enum MAPPING_TYPES { Specialties, Services, Classes, Videos }

        public ApiClientExtension(ApiClient client)
        {
            _client = client;
        }

        //public List<ContentResponse> SearchByServiceLineSlugs(string audienceSlug, List<>)

        public List<ContentResponse> GetRelatedContent(string bucketSlug, string contentSlug)
        {
            return GetRelatedContent(bucketSlug, contentSlug, new List<string>());
        }

        public List<ContentResponse> GetRelatedContent(string bucketSlug, string contentSlug, List<string> Languages)
        {
            //Start with an article that you want to get the related content for
            ContentArticleResponse article = _client.Content.GetContent(bucketSlug, contentSlug, new GetContentOptions());
            ContentTaxonomyList meshTaxonomy = article.Taxonomies.Find(c => c.Slug == "mesh");

            //If there aren't any mesh terms then we'll return no related documents
            if (meshTaxonomy == null) return new List<ContentResponse>();

            //Mesh terms exist, let's build a mesh basd query to get related articles.
            string query = "mesh: ";
            if (meshTaxonomy.Items.Count > 0) query = "mesh: " + meshTaxonomy.Items[0].Value;
            for (int i = 1; i < meshTaxonomy.Items.Count; i++)
            {
                query += " or mesh: " + meshTaxonomy.Items[i].Value;
            }

            //Search for content with an overlapping mesh code.
            ContentList relatedContent = _client.Content.SearchContent(new ContentSearchRequest
            {
                Query = query,
                Count = DEFAULT_COUNT,
                Languages = Languages
            });

            //Fiter out the item that we started the related to search from.
            List<ContentResponse> contentResponses = new List<ContentResponse>();
            foreach (ContentResponse response in relatedContent.Items)
            {
                if (response.Bucket.Slug != bucketSlug || response.Slug != contentSlug)
                {
                    contentResponses.Add(response);
                }
            }

            return contentResponses;
        }

        public List<ServiceLineMappingResponse> GetMappedServices(ContentArticleResponse content)
        {
            //Get the mappings
            List<ServiceLineMappingResponse> mappings = new List<ServiceLineMappingResponse>();
            foreach (ServiceLineResponse serviceLine in content.ServiceLines)
            {
                //There are 4 types of mappings that could be returned.  We must iterate over each.
                foreach (MAPPING_TYPES mappingType in Enum.GetValues(typeof(MAPPING_TYPES)))
                {
                    try
                    {
                        ServiceLineMappingResponseList mappingResponseList = _client.Mappings.GetServiceLineMappings(mappingType.ToString(), serviceLine.AudienceSlug, serviceLine.ServiceLineSlug, serviceLine.PageKeywordSlug, false);
                        mappings.AddRange(mappingResponseList.Items);
                    } catch (ServiceException ex)
                    {
                        //Not all clients purchase iMapping.  If iMapping is not part of the package simply return an empty list.
                        if (ex.StatusCode == System.Net.HttpStatusCode.Forbidden) return new List<ServiceLineMappingResponse>();
                        throw ex;
                    }  
                }
            }

            return mappings;
        }
    }
}