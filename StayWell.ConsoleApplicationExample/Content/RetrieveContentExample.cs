using System;
using StayWell.Client;
using StayWell.ServiceDefinitions.Content.Objects;

namespace StayWell.ConsoleApplicationExample.Content
{
    /// <summary>
    /// Samples to demonstrate retrieving an article from the API
    /// </summary>
    public class RetrieveContentExample
    {
        public ContentArticleResponse GetContent(ApiClient client, string bucket, string slug)
        {
            var article = client.Content.GetContent(bucket, slug, new GetContentOptions
            {
                //Setting this property to true will retrieve the body of the article. Default = false
                IncludeBody = true
            });
            Console.WriteLine("Article with Id {0} retrieved", article.Id);
            return article;
        }
    }
}