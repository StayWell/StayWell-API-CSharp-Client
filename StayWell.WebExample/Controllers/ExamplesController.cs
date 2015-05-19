using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

using StayWell.Client;
using StayWell.Interface;
using StayWell.ServiceDefinitions;
using StayWell.ServiceDefinitions.Content.Objects;
using StayWell.ServiceDefinitions.Buckets.Objects;

using StayWell.WebExample.Models;
using StayWell.ServiceDefinitions.Collections.Objects;


namespace StayWell.WebExample.Controllers
{
    public class ExamplesController : Controller
    {
        private const int DEFAULT_COUNT = 50;

        //Create an authenticated SW API client
        private ApiClient _client = new ApiClient(ConfigurationManager.AppSettings["ApplicationId"], ConfigurationManager.AppSettings["ApplicationSecret"]);
        
        #region Public Controller Actions

        //
        // GET: /Samples/DisplayContent
        public ActionResult DisplayContent()
        {
            //Request the specific article.  If you intend to display the full article you must send the flag "IncludeBody"
            ContentArticleResponse article = _client.Content.GetContent("diseases-and-conditions", "Prion-Diseases", new GetContentOptions
            {
                IncludeBody = true
            });

            return View(article);
        }

        //
        // GET: /Samples/DisplayRelatedContent
        public ActionResult DisplayRelatedContent()
        {
            RelatedContentModel model = new RelatedContentModel
            {
                OriginalArticleTitle = "Aortic Valve Regurgitation",
                RelatedContent = GetRelatedContent("diseases-and-conditions", "aortic-valve-regurgitation")
            };

            return View(model);
        }

        //
        // GET: /Samples/DisplayCollection
        public ActionResult DisplayCollection()
        {
            CollectionResponse collection = _client.Collections.GetCollection("development-sample-license",true,true,true);

            return View(collection);
        }

        //
        // GET: /Samples/DisplayCollections
        public ActionResult DisplayCollections()
        {
            CollectionListResponse collections = _client.Collections.SearchCollections(new CollectionSearchRequest
            {
                Count = DEFAULT_COUNT
            });

            return View(collections);

        }

        //
        // GET: /Samples/DisplayBuckets
        public ActionResult DisplayBuckets()
        {
            ContentBucketList buckets = _client.Buckets.SearchBuckets(new BucketSearchRequest
            {
                Count = DEFAULT_COUNT
            });

            return View(buckets);
        }

        //
        // GET: /Samples/SearchContent
        public ActionResult SearchContent(string searchString)
        {
            ContentList searchResults = new ContentList();
            
            if (!string.IsNullOrEmpty(searchString))
            {
                searchResults = _client.Content.SearchContent(new ContentSearchRequest
                {
                    Count = DEFAULT_COUNT,
                    Query = searchString

                });
            }

            return View(searchResults);
        }


        #endregion

        #region Private Helper Methods

        private List<ContentResponse> GetRelatedContent(string bucketSlug, string contentSlug)
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
                query += ", " + meshTaxonomy.Items[i].Value;
            }

            //Search for content with an overlapping mesh code.
            ContentList relatedContent = _client.Content.SearchContent(new ContentSearchRequest
            {
                Query = query,
                Count = DEFAULT_COUNT
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

        #endregion
    }
}