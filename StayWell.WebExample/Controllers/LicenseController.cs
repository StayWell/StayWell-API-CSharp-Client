using StayWell.Client;
using StayWell.ServiceDefinitions.Buckets.Objects;
using StayWell.ServiceDefinitions.Collections.Objects;
using StayWell.ServiceDefinitions.Content.Objects;
using StayWell.WebExample.App_Code;
using StayWell.WebExample.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StayWell.WebExample.Controllers
{
    public class LicenseController : Controller
    {
        private const int DEFAULT_COUNT = 100;
        private const int MAX_RESULTS = 10000;
        private const int MAX_DOWNLOAD_DISPLAY_COUNT = 500;
        public const string IS_PUBLIC_DEMO = "IsPublicDemo";
        private const string PRIVATE_HOST_NAME = "localhost";
        private const int TRIM_COUNT = 3;

        //Create an authenticated SW API client
        private ApiClient _client = new ApiClient(ConfigurationManager.AppSettings["ApplicationId"], ConfigurationManager.AppSettings["ApplicationSecret"]);

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            ViewData.Add(IS_PUBLIC_DEMO, true);

            var hostname = requestContext.HttpContext.Request.Url.Host;
            if (hostname == PRIVATE_HOST_NAME) ViewData[IS_PUBLIC_DEMO] = false;

            base.Initialize(requestContext);
        }

        #region Public Controller Actions

        //
        // GET: /License/DisplayCollections
        public ActionResult DisplayCollections()
        {
            CollectionListResponse collections = _client.Collections.SearchCollections(new CollectionSearchRequest
            {
                Count = DEFAULT_COUNT
            });

            //Trim the results if this is a public demo.
            if (IsPublicDemo())
            {
                List<CollectionResponse> trimmedList = new List<CollectionResponse>();
                trimmedList.AddRange(collections.Items.Take(TRIM_COUNT));

                collections.Items = trimmedList;
            }

            return View(collections);

        }

        //
        // GET: /License/DisplayBuckets
        public ActionResult DisplayBuckets()
        {
            ContentBucketList buckets = _client.Buckets.SearchBuckets(new BucketSearchRequest
            {
                Count = DEFAULT_COUNT
            });

            //Trim the results if this is a public demo.
            if (IsPublicDemo())
            {
                List<ContentBucketResponse> trimmedList = new List<ContentBucketResponse>();
                trimmedList.AddRange(buckets.Items.Take(TRIM_COUNT));

                buckets.Items = trimmedList;
            }

            return View(buckets);
        }

        //
        // GET: /License/DownloadAllContent
        public ActionResult DownloadAllContent()
        {
            return View();
        }

        //
        // GET: /License/DownloadContentFile
        public FileStreamResult DownloadContentFile()
        {
            //Trim results if this is a public demo.  Since this method can take a long time to run we will trim on the front end
            //so that we don't take the time to enumerate all documents in the license
           
            List<ContentReferenceModel> contentToDownload = new List<ContentReferenceModel>();
            if (IsPublicDemo()) contentToDownload = GetContentForPublicDemoDownload();
            else contentToDownload = GetContentForDownload();

            //At this point the content can be downloaded from the contentToDownload list.
            //Since the process of downloading the content is very system specific we are leaving
            //that part up to the implementer.

            //Create a file for download
            MemoryStream memoryStream = new MemoryStream();
            TextWriter tw = new StreamWriter(memoryStream);

            tw.WriteLine("Title,Bucket Slug,Content Slug, Legacy ID");
            foreach (var item in contentToDownload)
            {
                string legacyId = item.LegacyId;
                if (legacyId != null) legacyId = item.LegacyId.Replace(',', '-');
                else legacyId = string.Empty;

                tw.WriteLine(item.Title.Replace(",", "") + "," + item.BucketSlug + "," + item.Slug + "," + legacyId);
            }
            tw.Flush();
            memoryStream.Flush();
            memoryStream.Position = 0;

            FileStreamResult result = new FileStreamResult(memoryStream, "application/csv");
            result.FileDownloadName = "licensedownload.csv";
            
            return result;
        }

        //
        // GET: /License/DisplayServiceLines
        public ActionResult DisplayServiceLines(string audienceSlug, string serviceLineSlug)
        {
            ShowServiceLinesModel model = new ShowServiceLinesModel();

            ServiceLineResponseList audiences = _client.ServiceLines.GetAllAudiences();
            foreach (var item in audiences.Items)
            {
                model.Audiences.Add(item.AudienceSlug);
            }

            if (!string.IsNullOrEmpty(audienceSlug))
            {
                ServiceLineResponseList serviceLines = _client.ServiceLines.GetServiceLinesInAudience(audienceSlug);
                foreach (var item in serviceLines.Items)
                {
                    model.ServiceLines.Add(item.ServiceLineSlug);
                }
                model.audienceSlug = audienceSlug;
            }

            if ((!string.IsNullOrEmpty(audienceSlug)) && (!string.IsNullOrEmpty(serviceLineSlug)))
            {
                ServiceLineResponseList keywords = _client.ServiceLines.GetKeywordsInServiceLine(audienceSlug, serviceLineSlug);
                foreach (var item in keywords.Items)
                {
                    model.PageKeywords.Add(item.PageKeywordSlug);
                }
                model.serviceLineSlug = serviceLineSlug;
            }

            if (IsPublicDemo())
            {
                List<string> trimmedServiceLines = new List<string>();
                trimmedServiceLines.AddRange(model.ServiceLines.Take(TRIM_COUNT));
                model.ServiceLines = trimmedServiceLines;

                List<string> trimmedKeywords = new List<string>();
                trimmedKeywords.AddRange(model.PageKeywords.Take(TRIM_COUNT));
                model.PageKeywords = trimmedKeywords;
            }

            return View(model);
        }
        #endregion

        #region Download Content From License

        private List<ContentReferenceModel> GetContentForPublicDemoDownload()
        {
            List<ContentReferenceModel> trimmedContentToDownload = new List<ContentReferenceModel>();
            ContentBucketList buckets = SearchBuckets();
            if (buckets.Items.Count > 0)
            {
                ContentList contentList = new ContentList();
                contentList.Items = new List<ContentResponse>();
                for (int i = 0; i < buckets.Items.Count && i < TRIM_COUNT; i++)
                {
                    contentList.Items.AddRange(SearchContent(buckets.Items[i].Slug).Items);
                }
                List<ContentReferenceModel> contentForBucket = ConvertToContentModel(contentList);

                //Remove all duplicates.
                contentForBucket = RemoveDuplicates(contentForBucket);

                //Trim for public demo.
                trimmedContentToDownload.AddRange(contentForBucket.Take(TRIM_COUNT));
            }
            return trimmedContentToDownload;
        }

        private List<ContentReferenceModel> GetContentForDownload()
        {
            //Get all items from buckets
            ContentBucketList allBuckets = GetAllBuckets();
            ContentList contentFromBuckets = GetAllContentForBuckets(allBuckets);

            //Get all items from collection
            CollectionListResponse collections = GetAllCollections();
            List<CollectionItemResponse> collectionItemResponses = GetAllContentForCollections(collections);

            //Convert and combine everything into single model.
            List<ContentReferenceModel> contentToDownload = new List<ContentReferenceModel>();
            contentToDownload.AddRange(ConvertToContentModel(contentFromBuckets));
            contentToDownload.AddRange(ConvertToContentModel(collectionItemResponses));

            //Remove all duplicates.
            contentToDownload = RemoveDuplicates(contentToDownload);

            return contentToDownload;
        }

        #region Get Content for Buckets
        private ContentBucketList GetAllBuckets()
        {
            ContentBucketList allBuckets = new ContentBucketList();
            allBuckets.Items = new List<ContentBucketResponse>();

            ContentBucketList buckets = SearchBuckets();

            int offset = 0;
            int page = 0;
            bool isDoneTraversing = false;
            while (!isDoneTraversing)
            {
                allBuckets.Items.AddRange(buckets.Items);

                if (buckets.Items.Count == DEFAULT_COUNT)
                {
                    offset += DEFAULT_COUNT;
                    buckets = SearchBuckets(offset);
                    page++;
                }
                else isDoneTraversing = true;
            }

            return allBuckets;
        }
        private ContentList GetAllContentForBuckets(ContentBucketList buckets)
        {
            ContentList allContent = new ContentList();
            allContent.Items = new List<ContentResponse>();

            foreach (ContentBucketResponse bucket in buckets.Items)
            {
                if (bucket.Type != ContentType.Image)
                {
                    ContentList contentForBucket = GetAllContentForBucket(bucket.Slug);
                    allContent.Items.AddRange(contentForBucket.Items);
                }
            }
            return allContent;
        }

        private ContentList GetAllContentForBucket(string bucketSlug)
        {
            ContentList contentForBucket = new ContentList();
            contentForBucket.Items = new List<ContentResponse>();

            ContentList contentResponse = SearchContent(bucketSlug);
            if (contentResponse.Total > MAX_RESULTS)
            {
                ContentList contentForLargeBucket = GetAllContentForLargeBucket(bucketSlug);
                contentForBucket.Items.AddRange(contentForLargeBucket.Items);
            }
            else
            {
                int offset = 0;
                int page = 0;
                bool isDoneTraversing = false;
                while (!isDoneTraversing)
                {
                    contentForBucket.Items.AddRange(contentResponse.Items);

                    if (contentResponse.Items.Count == DEFAULT_COUNT)
                    {
                        offset += DEFAULT_COUNT;
                        contentResponse = SearchContent(bucketSlug, offset);
                        page++;
                    }
                    else isDoneTraversing = true;
                }
            }

            return contentForBucket;
        }

        private ContentList GetAllContentForLargeBucket(string bucketSlug)
        {
            ContentList contentForBucket = new ContentList();
            contentForBucket.Items = new List<ContentResponse>();

            List<string> firstLetterList = GetFirstLetterList();
            foreach(string firstLetter in firstLetterList)
            {
                ContentList contentResponse = SearchContent(bucketSlug, firstLetter.ToString());
                int offset = 0;
                int page = 0;
                bool isDoneTraversing = false;
                while (!isDoneTraversing)
                {
                    contentForBucket.Items.AddRange(contentResponse.Items);

                    if (contentResponse.Items.Count == DEFAULT_COUNT)
                    {
                        offset += DEFAULT_COUNT;
                        contentResponse = SearchContent(bucketSlug, firstLetter, offset);
                        page++;
                    }
                    else isDoneTraversing = true;
                }
            }

            return contentForBucket;
        }

        private ContentList SearchContent(string bucketSlug, string TitleStartsWith)
        {
            return SearchContent(bucketSlug, TitleStartsWith, 0);
        }

        private ContentList SearchContent(string bucketSlug, string titleStartsWith, int offset)
        {
            ContentList response = _client.Content.SearchContent(new ContentSearchRequest
            {
                Buckets = new List<string> { bucketSlug },
                Count = DEFAULT_COUNT,
                TitleStartsWith = titleStartsWith,
                Offset = offset
            });

            return response;
        }

        private ContentList SearchContent(string bucketSlug)
        {
            return SearchContent(bucketSlug, 0);
        }

        private ContentList SearchContent(string bucketSlug, int offset)
        {
            ContentList response = _client.Content.SearchContent(new ContentSearchRequest
            {
                Buckets = new List<string> { bucketSlug },
                Count = DEFAULT_COUNT,
                Offset = offset
            });

            return response;
        }

        private ContentBucketList SearchBuckets()
        {
            return SearchBuckets(0);
        }

        private ContentBucketList SearchBuckets(int offset)
        {
            ContentBucketList buckets = _client.Buckets.SearchBuckets(new BucketSearchRequest
            {
                Count = DEFAULT_COUNT,
                Offset = offset
            });

            return buckets;
        }

        private List<string> GetFirstLetterList()
        {
            List<string> firstLetterList = new List<string>();
            for (int i = 0; i < 10; i++) firstLetterList.Add(i.ToString());
            for (char c = 'A'; c <= 'Z'; c++) firstLetterList.Add(c.ToString());
            firstLetterList.AddRange(new List<string>
            {
                "\"",
                "'",
                "-",
                "_"
            });

            return firstLetterList;
        }


        #endregion

        #region Get Content for Collections
        private List<CollectionItemResponse> GetAllContentForCollections(CollectionListResponse collections)
        {
            List<CollectionItemResponse> contentList = new List<CollectionItemResponse>();

            foreach (CollectionResponse collection in collections.Items)
            {
                List<CollectionItemResponse> collectionContent = GetAllContentForCollection(collection.Slug);
                contentList.AddRange(collectionContent);
            }

            return contentList;
        }

        private List<CollectionItemResponse> GetAllContentForCollection(string collectionSlug)
        {
            CollectionResponse collection = _client.Collections.GetCollection(collectionSlug, true, true, true);
            List<CollectionItemResponse> flattenedCollectionList = GetFlattenedListOfContent(collection.Items);

            return flattenedCollectionList;
        }

        private List<CollectionItemResponse> GetFlattenedListOfContent(List<CollectionItemResponse> items)
        {
            List<CollectionItemResponse> flattenedContentList = new List<CollectionItemResponse>();
            foreach (var item in items)
            {
                if (item.Items == null || item.Items.Count == 0)
                {
                    if (item.Type == CollectionItemType.Content) flattenedContentList.Add(item);
                }
                else
                {
                    flattenedContentList.AddRange(GetFlattenedListOfContent(item.Items));
                }
            }
            return flattenedContentList;
        }

        private CollectionListResponse GetAllCollections()
        {
            CollectionListResponse allCollections = new CollectionListResponse();
            allCollections.Items = new List<CollectionResponse>();

            CollectionListResponse collections = SearchCollections();

            int offset = 0;
            int page = 0;
            bool isDoneTraversing = false;
            while (!isDoneTraversing)
            {
                allCollections.Items.AddRange(collections.Items);

                if (collections.Items.Count == DEFAULT_COUNT)
                {
                    offset += DEFAULT_COUNT;
                    collections = SearchCollections(offset);
                    page++;
                }
                else isDoneTraversing = true;
            }

            return allCollections;
        }

        private CollectionListResponse SearchCollections()
        {
            return SearchCollections(0);
        }

        private CollectionListResponse SearchCollections(int offset)
        {
            CollectionListResponse collections = _client.Collections.SearchCollections(new CollectionSearchRequest
            {
                Count = DEFAULT_COUNT,
                Offset = offset
            });

            return collections;
        }
        #endregion

        #region Convert and Trim Methods
        private List<ContentReferenceModel> ConvertToContentModel(ContentList contentList)
        {
            List<ContentReferenceModel> contentToDownload = new List<ContentReferenceModel>();

            foreach (ContentResponse item in contentList.Items)
            {
                contentToDownload.Add(new ContentReferenceModel
                {
                    BucketSlug = item.Bucket.Slug,
                    Type = item.Type.ToString(),
                    Slug = item.Slug,
                    Title = item.Title,
                    LegacyId = item.LegacyId
                });
            }

            return contentToDownload;
        }

        private List<ContentReferenceModel> ConvertToContentModel(List<CollectionItemResponse> collectionItems)
        {
            List<ContentReferenceModel> contentToDownload = new List<ContentReferenceModel>();

            foreach (CollectionItemResponse item in collectionItems)
            {
                contentToDownload.Add(new ContentReferenceModel
                {
                    BucketSlug = item.Bucket.Slug,
                    Type = item.Type.ToString(),
                    Slug = item.Slug,
                    Title = item.Title,
                    LegacyId = item.LegacyId
                });
            }

            return contentToDownload;
        }

        private List<ContentReferenceModel> RemoveDuplicates(List<ContentReferenceModel> items)
        {
            List<ContentReferenceModel> deDupedList = new List<ContentReferenceModel>();
            foreach (ContentReferenceModel item in items)
            {
                if (!deDupedList.Exists(c => c.Slug == item.Slug && c.BucketSlug == item.BucketSlug))
                {
                    deDupedList.Add(item);
                }
            }
            return deDupedList;
        }

        #endregion

        #endregion

        #region Private Methods
        private bool IsPublicDemo()
        {
            return (bool)ViewData[IS_PUBLIC_DEMO];
        }
        #endregion
    }
}