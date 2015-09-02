using StayWell.Client;
using StayWell.ServiceDefinitions.Content.Objects;
using StayWell.WebExample.App_Code;
using StayWell.WebExample.App_Code.Mappers;
using StayWell.WebExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StayWell.WebExample.Api
{
    public class VideoLibraryController : ApiController
    {
        private const int VIDEOS_PER_PAGE = 4;

        private ApiClient _client = StayWellAPIClientFactory.GetApiClient();

        [Route("api/videolibrary/{category}")]
        public GroupedContentModel Get(string category, int skip = 0, int top = 4)
        {
            GroupedContentModel groupedContent = new GroupedContentModel();

            ContentCategoryList categoryList = ContentCategoryFactory.GetVideoLibraryCategories();

            ContentReferenceModelMapper mapper = new ContentReferenceModelMapper();
            Category mappedCategory = categoryList.Categories.Find(c => c.Slug.Equals(category, StringComparison.InvariantCultureIgnoreCase));

            //Create the query
            string query = GetQueryStringFromServiceLines(mappedCategory);

            //Execute the query
            ContentList videos = _client.Content.SearchContent(new ContentSearchRequest
            {
                Count = top,
                Offset = skip,
                Buckets = new List<string> { "videos-v2" },
                Languages = new List<string> { "en" },
                Query = query
            });

            //Create the model
            if (videos.Items.Count > 0)
            {
                groupedContent = new GroupedContentModel
                {
                    Slug = mappedCategory.Slug,
                    Title = mappedCategory.Names.Find(c => c.LanguageCode == "en").Value,
                    Total = videos.Total
                };

                foreach (var video in videos.Items)
                {
                    groupedContent.Items.Add(mapper.Map(video));
                }

                //if (videos.Total > 4)
                //{
                //    groupedContent.IsMoreItems = true;
                //    for (int i = 4; i < videos.Total; i++)
                //    { 
                //        groupedContent.Items.Add(new ContentReferenceModel{
                //             Title = "Loading..."
                //        });
                //    }
                //}
            }

            return groupedContent;
        }

        // GET api/<controller>/cardiovascular?page={1}
        public string Get(string category, string page)
        {
            return "value";
        }

        #region Private Methods
        private string GetQueryStringFromServiceLines(Category category)
        {
            //Create the query
            string query = string.Empty;
            if (category.ServiceLines.Count > 0) query = "service-line: adult\\/" + category.ServiceLines[0].Slug + "\\/*";
            for (int i = 1; i < category.ServiceLines.Count; i++)
            {
                query += " OR service-line: adult\\/" + category.ServiceLines[i].Slug + "\\/*";
            }

            return query;
        }
        #endregion
    }
}