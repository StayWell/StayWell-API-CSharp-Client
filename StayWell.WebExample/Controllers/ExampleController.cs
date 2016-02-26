using StayWell.Client;
using StayWell.ServiceDefinitions.Content.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StayWell.WebExample.App_Code;
using StayWell.WebExample.Models;
using StayWell.ServiceDefinitions.Mapping.Objects;
using StayWell.Interface;
using System.Globalization;
using System.Text.RegularExpressions;
using StayWell.WebExample.App_Code.Mappers;
using HtmlAgilityPack;
using StayWell.ServiceDefinitions.Collections.Objects;

namespace StayWell.WebExample.Controllers
{
    public class ExampleController : Controller
    {
        private const int DEFAULT_COUNT = 50;

        //Create an authenticated SW API client
        private ApiClient _client = StayWellAPIClientFactory.GetApiClient();

        #region Public Controller Actions

        //
        // GET: /Display Article\Video/
        public ActionResult Index()
        {
            ContentWithRelationshipModel model = new ContentWithRelationshipModel();

            //Try to get the slug objects
            object bucketSlug = ControllerContext.RouteData.Values["bucketSlug"];
            object contentSlug = ControllerContext.RouteData.Values["contentSlug"];
            if (bucketSlug == null || contentSlug == null) return HttpNotFound("Sorry, the content was not found.");

            //Ensure the slugs are valid
            string bucketSlugValue = bucketSlug.ToString();
            string contentSlugValue = contentSlug.ToString();
            if (string.IsNullOrEmpty(bucketSlugValue)) return HttpNotFound("Sorry, the content was not found.");
            if (string.IsNullOrEmpty(contentSlugValue)) return HttpNotFound("Sorry, the content was not found.");

            //Request the specific article.  If you intend to display the full article you must send the flag "IncludeBody"
            try
            {
                model.Article = _client.Content.GetContent(bucketSlugValue, contentSlugValue, new GetContentOptions
                {
                    IncludeBody = true
                });

                //If the article is streaming media then we need to get more details about the content
                //so that we can render the video tag.
                if (model.Article.Type == ContentType.StreamingMedia)
                {
                    model.StreamingMedia = _client.StreamingMedia.GetStreamingMedia(bucketSlugValue, contentSlugValue, new GetContentOptions
                    {
                        IncludeBody = true
                    }, false);
                }

                //If segments are present convert all internal links to real links
                if (model.Article.Segments != null)
                {
                    ContentLinkConverter linkConverter = new ContentLinkConverter();
                    foreach (ContentSegmentResponse segment in model.Article.Segments)
                    {
                        segment.Body = linkConverter.ConvertContentLinksToRealLinks(segment.Body);
                    }
                }
            }
            catch (ServiceException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound) return HttpNotFound("Sorry, the content was not found.");
                else throw ex;
            }

            //Get the relations
            model.RelatedContent = GetRelatedContentGrouped(model.Article);
            model.RelatedContent.AddRange(GetRelatedServicesGrouped(model.Article));

            return View(model);
        }

        public ActionResult Captions()
        {
            //Prerequisites:
            //1) Web.Config Configuration: Default .net will not process vtt requests through MVC routing.  The following configuration is required
            //in the web.config to allow vtt files to be routed to the MVC controller.
            //<system.webServer>
            //  <modules runAllManagedModulesForAllRequests="true" />
            //</system.webServer>
            //2) A new MVC route was added to App_Start.RouteConfig.cs to route vtt files to this controller.

            //Try to get the slug objects
            object bucketSlug = ControllerContext.RouteData.Values["bucketSlug"];
            object contentSlug = ControllerContext.RouteData.Values["contentSlug"];
            if (bucketSlug == null || contentSlug == null) return HttpNotFound("Sorry, the content was not found.");

            //Ensure the slugs are valid
            string bucketSlugValue = bucketSlug.ToString();
            string contentSlugValue = contentSlug.ToString();
            if (string.IsNullOrEmpty(bucketSlugValue)) return HttpNotFound("Sorry, the content was not found.");
            if (string.IsNullOrEmpty(contentSlugValue)) return HttpNotFound("Sorry, the content was not found.");

            StreamingMediaResponse media = _client.StreamingMedia.GetStreamingMedia(bucketSlugValue, contentSlugValue, new GetContentOptions
            {
                IncludeBody = true
            }, false);
            return Content(media.ClosedCaptioning, "application/ttaf+xml");
        }

        public ActionResult Anatomy3D()
        {
            return View();
        }

        public ActionResult CenterPlugin()
        {
            return View();
        }

        public ActionResult Centers(string centerCategorySlug)
        {
            return View();
        }

        public ActionResult Videos()
        {
            return View();
        }

        public ActionResult AtoZPlugin()
        {
            return View();
        }

        public ActionResult TopicExplorer(string collectionSlug, string bucketSlug, string contentSlug)
        {
            CollectionResponse response = _client.Collections.GetCollection(collectionSlug, false, false, false);
            ViewData["Title"] = response.Title;
            
            return View();
        }

        public ActionResult VideosByTopic()
        {
            //Try to get the slug objects
            object categorySlug = ControllerContext.RouteData.Values["categorySlug"];
            if (categorySlug == null) return HttpNotFound("Sorry, the content was not found.");
            string categorySlugValue = categorySlug.ToString();
            if (string.IsNullOrEmpty(categorySlugValue)) return HttpNotFound("Sorry, the content was not found.");

            //Select the appropriate category
            ContentCategoryList categoryList = ContentCategoryFactory.GetVideoLibraryCategories();
            Category category = categoryList.Categories.Find(c => c.Slug == categorySlugValue);

            //Create the query
            string query = GetQueryStringFromServiceLines(category);

            ContentList videos = _client.Content.SearchContent(new ContentSearchRequest
                {
                    Count = DEFAULT_COUNT,
                    Buckets = new List<string> { "videos-v2" },
                    Languages = new List<string> { "en" },
                    Query = query
                });

            //Convert to the view model.
            GroupedContentModel model = new GroupedContentModel();
            model.Title = category.Names.Find(c => c.LanguageCode == "en").Value;
            model.Slug = category.Slug;

            ContentReferenceModelMapper mapper = new ContentReferenceModelMapper();
            foreach (var item in videos.Items)
            {
                model.Items.Add(mapper.Map(item));
            }

            return View(model);
        }
        #endregion

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

        private List<GroupedContentModel> GetRelatedServicesGrouped(ContentArticleResponse content)
        {
            //Use the helper extension to get related service lines.
            ApiClientExtension extension = new ApiClientExtension(_client);
            List<ServiceLineMappingResponse> mappings = extension.GetMappedServices(content);

            //Map the related services to the grouped content model
            MappingToGroupedContentModelMapper mapper = new MappingToGroupedContentModelMapper();
            List<GroupedContentModel> groupedContent = mapper.Map(mappings);

            return groupedContent;
        }

        private List<GroupedContentModel> GetRelatedContentGrouped(ContentArticleResponse content)
        {
            //Use the helper extension to get related content.
            ApiClientExtension extension = new ApiClientExtension(_client);
            List<ContentResponse> relatedContent = extension.GetRelatedContent(content.Bucket.Slug, content.Slug, new List<string> { "En" });

            //Map the related content to the grouped content model
            ContentToGroupedContentModelMapper mapper = new ContentToGroupedContentModelMapper();
            List<GroupedContentModel> groupedContent = mapper.Map(relatedContent);

            return groupedContent;
        }
        #endregion
    }
}