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

        private ApiClient _client = StayWellAPIClientFactory.GetApiClient();

        [Route("api/videolibrary")]
        public GroupedContentModel Get( int skip = 0, int top = 4)
        {
            //Model used for the view
            var groupedContent = new GroupedContentModel();
            //Service lines must include an audience, service line, and keyword separated by a '/'; for example: \"adult/diabetes/diabetes
            //Create a ContentSearchRequest object to search content
            var videos = _client.Content.SearchContent(new ContentSearchRequest
            {
                Count = top,
                Offset = skip,
                Languages = new List<string> { "en" },
                ServiceLines = new List<string>()
                {
                    "adult/gastroenterology/hepatitis-c",
                    "adult/infectious-disease/hepatitis-c"
                }
        });

            //Mapper to map content for viewing and retreiving body of streaming media
            var mapper = new ContentReferenceModelMapper();

            //Create the model
            if (videos.Items.Count > 0)
            {
                groupedContent = new GroupedContentModel
                {
                    Slug = "hepatitis",
                    Title = "Hepatitis",
                    Total = videos.Total
                };

                foreach (var video in videos.Items)
                {
                    groupedContent.Items.Add(mapper.Map(video));
                }
            }

            return groupedContent;
        }


    }
}