using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StayWell.Client;
using StayWell.ServiceDefinitions.Content.Objects;
using System.Configuration;

namespace StayWell.WebExample.App_Code
{
    public static class StayWellAPIClientFactory
    {
        
        public static ApiClient GetApiClient()
        {
            return new ApiClient(ConfigurationManager.AppSettings["ApplicationId"], ConfigurationManager.AppSettings["ApplicationSecret"]);
        }
    }
}