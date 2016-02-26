using StayWell.Client;
using StayWell.ServiceDefinitions.Collections.Objects;
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
    public class CollectionsController : ApiController
    {
        private ApiClient _client = StayWellAPIClientFactory.GetApiClient();

        [Route("api/collections/{collectionSlug}")]
        public CollectionResponse Get(string collectionSlug)
        {
            //Execute the query
            CollectionResponse collection = _client.Collections.GetCollection(collectionSlug, true, true, true);
            return collection;
        }

        [Route("api/collectionslite/{collectionSlug}")]
        public CollectionResponse GetLite(string collectionSlug)
        {
            //Execute the query
            CollectionResponse collection = _client.Collections.GetCollection(collectionSlug, true, false, false);
            return collection;
        }
    }
}