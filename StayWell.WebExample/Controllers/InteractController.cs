using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

using StayWell.Client;
using StayWell.Interface;
using StayWell.ServiceDefinitions;
using StayWell.ServiceDefinitions.Content.Objects;
using StayWell.ServiceDefinitions.Buckets.Objects;

using StayWell.WebExample.Models;
using StayWell.ServiceDefinitions.Collections.Objects;


namespace StayWell.WebExample.Controllers
{
    public class InteractController : Controller
    {
        private const int DEFAULT_COUNT = 50;

        //Create an authenticated SW API client
        private ApiClient _client = new ApiClient(ConfigurationManager.AppSettings["ApplicationId"], ConfigurationManager.AppSettings["ApplicationSecret"]);

        #region Public Controller Actions

        //
        // GET: /Samples/SearchContent
        public ActionResult SearchContent(string searchString)
        {
            ContentList searchResults = new ContentList();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchResults = _client.Content.SearchContent(new ContentSearchRequest
                {
                    Count = DEFAULT_COUNT,
                    Query = searchString
                });
            }

            return View(searchResults);
        }

        #endregion

    }
}