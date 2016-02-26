using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StayWell.WebExample
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "VideoLibraryCategory",
              url: "Content/Videos/{categorySlug}",
              defaults: new { controller = "Example", action = "VideosByTopic", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "VideoLibrary",
               url: "Content/Videos/",
               defaults: new { controller = "Example", action = "Videos", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "ClosedCaptioning",
                url: "Content/{bucketSlug}/{contentSlug}/Captions.dfxp.xml",
                defaults: new { controller = "Example", action = "Captions", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "TopicExplorer",
               url: "TopicArea/{collectionSlug}/{bucketSlug}/{contentSlug}",
               defaults: new { controller = "Example", action = "TopicExplorer", bucketSlug = UrlParameter.Optional, contentSlug = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "Centers",
              url: "Centers/{*folderPath}",
              defaults: new { controller = "Example", action = "Centers" }
          );

            routes.MapRoute(
               name: "Content",
               url: "Content/{bucketSlug}/{contentSlug}",
               defaults: new { controller = "Example", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}
