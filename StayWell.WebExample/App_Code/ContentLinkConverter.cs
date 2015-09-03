using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StayWell.WebExample.App_Code
{
    public class ContentLinkConverter
    {
        private const string BUCKET_SLUG_KEY = "{{BucketSlug}}";
        private const string CONTENT_SLUG_KEY = "{{ContentSlug}}";

        private string _linkPattern = String.Format("/Content/{0}/{1}",BUCKET_SLUG_KEY, CONTENT_SLUG_KEY);

        /// <summary>
        /// Class to help convert all internal content links to real links within the hosting site.
        /// </summary>
        public ContentLinkConverter()
        { }
        
        /// <summary>
        /// Class to help convert all internal content links to real links within the hosting site.
        /// </summary>
        /// <param name="linkPattern">String pattern for rendering content.  Example: /Content/{{BucketSlug}}/{{ContentSlug}}</param>
        public ContentLinkConverter(string linkPattern)
        {
                _linkPattern = linkPattern;
        }
        public string ConvertContentLinksToRealLinks(string contentBody)
        {
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(contentBody);

            HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//a[@data-content-slug]");
            if (nodes != null)
            {
                foreach (HtmlNode linkNode in nodes)
                {
                    HtmlAttribute contentSlug = linkNode.Attributes["data-content-slug"];
                    HtmlAttribute bucketSlug = linkNode.Attributes["data-bucket-slug"];

                    string contentUri = _linkPattern.Replace(BUCKET_SLUG_KEY, bucketSlug.Value);
                    contentUri = contentUri.Replace(CONTENT_SLUG_KEY, contentSlug.Value);

                    //This line should reflect the path you use to display your content.
                    linkNode.Attributes.Add("href", contentUri);
                }
            }

            return htmlDoc.DocumentNode.OuterHtml;
        }
    }
}