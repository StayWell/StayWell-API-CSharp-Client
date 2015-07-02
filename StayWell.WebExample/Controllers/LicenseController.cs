using StayWell.Client;
using StayWell.ServiceDefinitions.Buckets.Objects;
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
    public class LicenseController : Controller
    {
        private const int DEFAULT_COUNT = 100;
        private const int MAX_RESULTS = 10000;
        private const int MAX_DOWNLOAD_DISPLAY_COUNT = 500;

        //Create an authenticated SW API client
        private ApiClient _client = new ApiClient(ConfigurationManager.AppSettings["ApplicationId"], ConfigurationManager.AppSettings["ApplicationSecret"]);

        #region Public Controller Actions

        //
        // GET: /License/DisplayCollections
        public ActionResult DisplayCollections()
        {
            CollectionListResponse collections = _client.Collections.SearchCollections(new CollectionSearchRequest
            {
                Count = DEFAULT_COUNT
            });

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

            return View(buckets);
        }

        //
        // GET: /License/DownloadAllContent
        public ActionResult DownloadAllContent()
        {
            //Get all items from buckets
            ContentBucketList allBuckets = GetAllBuckets();
            ContentList contentFromBuckets = GetAllContentForBuckets(allBuckets);

            //Get all items from collection
            CollectionListResponse collections = GetAllCollections();
            List<CollectionItemResponse> collectionItemResponses = GetAllContentForCollections(collections);
            
            //Convert and combine everything into single model.
            List<ContentModel> contentToDownload = new List<ContentModel>();
            contentToDownload.AddRange(ConvertToContentModel(contentFromBuckets));
            contentToDownload.AddRange(ConvertToContentModel(collectionItemResponses));

            //Trim the results so that the page can properly render the list.
            contentToDownload = TrimContentListForWebDisplay(contentToDownload);

            //return View(contentToDownload);
            return View(contentToDownload);
        }

        #endregion

        #region Download Content From License

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

            for (char c = 'A'; c <= 'Z'; c++)
            {
                ContentList contentResponse = SearchContent(bucketSlug, c.ToString());
                int offset = 0;
                int page = 0;
                bool isDoneTraversing = false;
                while (!isDoneTraversing)
                {
                    contentForBucket.Items.AddRange(contentResponse.Items);

                    if (contentResponse.Items.Count == DEFAULT_COUNT)
                    {
                        offset += DEFAULT_COUNT;
                        contentResponse = SearchContent(bucketSlug, c.ToString(), offset);
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
                if (item.Items==null || item.Items.Count == 0)
                {
                    if(item.Type == CollectionItemType.Content) flattenedContentList.Add(item);
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
        private List<ContentModel> ConvertToContentModel(ContentList contentList)
        {
            List<ContentModel> contentToDownload = new List<ContentModel>();

            foreach (ContentResponse item in contentList.Items)
            {
                contentToDownload.Add(new ContentModel
                {
                    BucketSlug = item.Bucket.Slug,
                    Type = item.Type.ToString(),
                    Slug = item.Slug,
                    Title = item.Title
                });
            }

            return contentToDownload;
        }

        private List<ContentModel> ConvertToContentModel(List<CollectionItemResponse> collectionItems)
        {
            List<ContentModel> contentToDownload = new List<ContentModel>();

            foreach (CollectionItemResponse item in collectionItems)
            {
                contentToDownload.Add(new ContentModel
                {
                    BucketSlug = item.Bucket.Slug,
                    Type = item.Type.ToString(),
                    Slug = item.Slug,
                    Title = item.Title
                });
            }

            return contentToDownload;
        }

        private List<ContentModel> TrimContentListForWebDisplay(List<ContentModel> contentListToTrim)
        {
            if (contentListToTrim.Count > MAX_DOWNLOAD_DISPLAY_COUNT)
            {
                List<ContentModel> trimmedContentToDisplay = new List<ContentModel>();
                for (int i = 0; i < MAX_DOWNLOAD_DISPLAY_COUNT; i++)
                {
                    trimmedContentToDisplay.Add(contentListToTrim[i]);
                }

                contentListToTrim = trimmedContentToDisplay;
            }

            return contentListToTrim;
        }
        #endregion

        #endregion

    }
}