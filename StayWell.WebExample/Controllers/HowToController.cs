using StayWell.Client;
using StayWell.ServiceDefinitions.Collections.Objects;
using StayWell.ServiceDefinitions.Content.Objects;
using StayWell.WebExample.App_Code;
using StayWell.WebExample.Models;
using System;
using System.Configuration;
using System.Web.Mvc;
using HtmlAgilityPack;

namespace StayWell.WebExample.Controllers
{
	public class HowToController : Controller
	{
		private const int DEFAULT_COUNT = 50;

		//Create an authenticated SW API client
		private ApiClient _client = new ApiClient(ConfigurationManager.AppSettings["ApplicationId"], ConfigurationManager.AppSettings["ApplicationSecret"]);

		#region Public Controller Actions

		//
		// GET: /HowTo/DisplayArticle
		public ActionResult DisplayArticle()
		{
			//Request the specific article.  If you intend to display the full article you must send the flag "IncludeBody"
			ContentArticleResponse article = _client.Content.GetContent("diseases-and-conditions", "Prion-Diseases", new GetContentOptions
			{
				IncludeBody = true
			});

			return View(article);
		}

		//
		// GET: /HowTo/DisplayRelatedContent
		public ActionResult DisplayRelatedContent()
		{

			ApiClientExtension clientExtension = new ApiClientExtension(_client);

			RelatedContentModel model = new RelatedContentModel
			{
				OriginalArticleTitle = "Absence Seizures",
				RelatedContent = clientExtension.GetRelatedContent("diseases-and-conditions", "absence-seizures")
			};

			return View(model);
		}

		//
		// GET: /HowTo/DisplayContent
		public ActionResult DisplayVideo()
		{
			//Request the specific article.  If you intend to display the full article you must send the flag "IncludeBody"
			StreamingMediaResponse video = _client.StreamingMedia.GetStreamingMedia("videos-v2", "carpal-tunnel-syndrome", new GetContentOptions
			{
				IncludeBody = true,
				EditMode = false
			});

			return View(video);
		}


		//
		// GET: /HowTo/DisplayCollection
		public ActionResult DisplayCollection()
		{
			CollectionResponse collection = _client.Collections.GetCollection("development-sample-license", true, true, true);

			return View(collection);
		}

		//
		// GET: /HowTo/DisplayJSONP
		public ActionResult DisplayJSONP()
		{
			//With JSONP all requets occur client to server.
			ViewData.Add("AppId", ConfigurationManager.AppSettings["ApplicationId"]);

			return View();
		}

		//
		// GET: /HowTo/DisplayArticleLink
		public ActionResult DisplayArticleLink()
		{
			//Request the specific article.  If you intend to display the full article you must send the flag "IncludeBody"
			ContentArticleResponse article = _client.Content.GetContent("development-sample-articles", "link-article-1", new GetContentOptions
			{
				IncludeBody = true
			});

			//Process all segments and convert any internal content links to real links.
			foreach (ContentSegmentResponse segment in article.Segments)
			{
				HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
				htmlDoc.LoadHtml(segment.Body);

				HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//a[@data-content-slug]");
				if (nodes != null)
				{
					foreach (HtmlNode linkNode in nodes)
					{
						HtmlAttribute contentSlug = linkNode.Attributes["data-content-slug"];
						HtmlAttribute bucketSlug = linkNode.Attributes["data-bucket-slug"];

						//This line should reflect the path you use to display your content.
						linkNode.Attributes.Add("href", "/Content/" + bucketSlug.Value + "/" + contentSlug.Value);
					}
				}

				segment.Body = htmlDoc.DocumentNode.OuterHtml;
			}

			return View(article);
		}

		//
		// GET: /HowTo/ConvertImagePopup
		public ActionResult ConvertImagePopup()
		{
			//Request the specific article.  If you intend to display the full article you must send the flag "IncludeBody"
			ContentArticleResponse article = _client.Content.GetContent("development-sample-articles", "image-article-1", new GetContentOptions
			{
				IncludeBody = true
			});

			//Process all segments and convert any internal content links to real links.
			foreach (ContentSegmentResponse segment in article.Segments)
			{
				HtmlDocument htmlDoc = new HtmlDocument();
				htmlDoc.LoadHtml(segment.Body);

				HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//img");
				if (nodes != null)
				{
					foreach (HtmlNode imgNode in nodes)
					{
						// get the parent nodes and validate
						var a = imgNode.ParentNode;
						if (a.Name != "a") continue;
						var container = a.ParentNode;
						if (container.Name != "div") continue;

						// get the attributes needed for the JavaScript pop-up
						var href = a.GetAttributeValue("href", "");
						var width = container.GetAttributeValue("data-width", "");
						var height = container.GetAttributeValue("data-height", "");

						// add an onclick handler to the a tag
						a.SetAttributeValue("onclick", string.Format(
							"javascript:return InitPopup('{0}'," +
							"'toolbar=no,location=no,status=no," +
							"menubar=no,resizable=no,scrollbars=yes," +
							"left=80,top=80,width={1},height={2}');",
							href, width, height));
					}
				}

				segment.Body = htmlDoc.DocumentNode.OuterHtml;
			}

			return View(article);
		}

		public ActionResult SearchByPublishedDate()
		{
			DateTime fromDate = DateTime.Now.AddDays(-180);

			string searchString = "published-date:>=" + fromDate.ToString("yyyy-MM-DD");
			ContentList searchResults = _client.Content.SearchContent(new ContentSearchRequest
			{
				Count = 5,
				Query = searchString
			});

			//Create the model
			SearchByPublishedDateModel model = new SearchByPublishedDateModel
			{
				FromDate = fromDate,
				Items = searchResults.Items
			};

			return View(model);
		}

		//
		// GET: /HowTo/SearchContent
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
	}
}