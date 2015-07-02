using StayWell.Client;
using StayWell.ServiceDefinitions.Collections.Objects;
using StayWell.ServiceDefinitions.Content.Objects;
using StayWell.WebExample.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StayWell.WebExample.Controllers
{
    public class DisplayController : Controller
    {
        private const int DEFAULT_COUNT = 50;

        //Create an authenticated SW API client
        private ApiClient _client = new ApiClient(ConfigurationManager.AppSettings["ApplicationId"], ConfigurationManager.AppSettings["ApplicationSecret"]);

        #region Public Controller Actions

        //
        // GET: /Display/DisplayContent
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
        // GET: /Display/DisplayRelatedContent
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
        // GET: /Display/DisplayCollection
        public ActionResult DisplayCollection()
        {
            CollectionResponse collection = _client.Collections.GetCollection("development-sample-license", true, true, true);

            return View(collection);
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