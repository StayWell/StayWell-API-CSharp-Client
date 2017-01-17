using StayWell.ServiceDefinitions.Content.Objects;
using StayWell.WebExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using StayWell.Client;

namespace StayWell.WebExample.App_Code.Mappers
{
    public class ContentReferenceModelMapper : IMapToNew<ContentResponse,ContentReferenceModel>
    {
        private ApiClient _client;
        public ContentReferenceModelMapper()
        { 
            _client = StayWellAPIClientFactory.GetApiClient();
        }

        public ContentReferenceModel Map(ContentResponse source)
        {
            ContentReferenceModel contentModel = new ContentReferenceModel
            {
                BucketSlug = source.Bucket.Slug,
                Slug = source.Slug,
                Title = source.Title,
                Type = source.Type.ToString()
            };

            string htmlFreeBlurb = Regex.Replace(source.Blurb, @"<[^>]+>|&nbsp;", "").Trim();
            contentModel.Blurb = Regex.Replace(htmlFreeBlurb, @"\s{2,}", " ");

            if (source.Type == ContentType.StreamingMedia)
            {
                StreamingMediaResponse media = _client.StreamingMedia.GetStreamingMedia(source.Bucket.Slug, source.Slug, new GetContentOptions
                {
                    IncludeBody = true,
                    EditMode = true
                });


                int seconds = media.Formats.First().RunningTime % 60;
                int minutes = media.Formats.First().RunningTime / 60;
                
                contentModel.Duration = minutes + ":" + seconds.ToString("D2");
                contentModel.ImageUri = media.ImageUri;
                contentModel.Rating = media.Rating;
                contentModel.RatingsCount = media.RatingsCount;
                contentModel.ViewCount = media.ViewCount;
            }

            return contentModel;
        }

    }
}