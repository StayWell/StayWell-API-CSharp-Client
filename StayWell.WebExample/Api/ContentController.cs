using StayWell.Client;
using StayWell.Interface;
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
    public class ContentController : ApiController
    {
        private ApiClient _client = StayWellAPIClientFactory.GetApiClient();

        [Route("api/content/{bucketSlug}/{contentSlug}")]
        public ContentArticleResponse Get(string bucketSlug, string contentSlug)
        {
            //Execute the query
            ContentArticleResponse content;
            try
            {
                content = _client.Content.GetContent(bucketSlug, contentSlug, new GetContentOptions
                {
                    IncludeBody = true
                });
            }
            catch (ServiceException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new Exception("Content not available in license.  The default license used in the example is a demo license with limited content.  Replace the license information in the web.config with your specific license information to see the full experience.", ex);
                }
                else throw;
            }

            if (content.Segments != null)
            {
                ContentLinkConverter linkConverter = new ContentLinkConverter();
                foreach (var segment in content.Segments)
                {
                    segment.Body = linkConverter.ConvertContentLinksToRealLinks(segment.Body);
                }
            }

            return content;
        }
    }
}