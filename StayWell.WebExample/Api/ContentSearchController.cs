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
    public class ContentSearchController : ApiController
    {
        private ApiClient _client = StayWellAPIClientFactory.GetApiClient();

        [Route("api/contentsearch/{top:int?}/{skip:int?}")]
        public ContentList Get(int top = 50, int skip = 0)
        {
            return Get(null, null, top, skip);
        }

        [Route("api/contentsearch/{titleStartsWith}/{top:int?}/{skip:int?}")]
        public ContentList Get(string titleStartsWith, int top = 50, int skip = 0)
        {
            return Get(titleStartsWith, null, top, skip);
        }

        [Route("api/contentsearch/{titleStartsWith}/{buckets}/{top:int?}/{skip:int?}")]
        public ContentList Get(string titleStartsWith, string buckets, int top = 50, int skip = 0)
        {
            List<string> bucketList = null;
            if(!string.IsNullOrEmpty(buckets))
            {
                bucketList = buckets.Split(',').ToList<string>();
            }

            ContentSearchRequest request = new ContentSearchRequest();
            if (!string.IsNullOrEmpty(titleStartsWith)) request.TitleStartsWith = titleStartsWith;
            if (bucketList != null) request.Buckets = bucketList;
            request.Count = top;
            request.Offset = skip;

            //Execute the query
            ContentList response = _client.Content.SearchContent(request);

            return response;
        }
    }
}