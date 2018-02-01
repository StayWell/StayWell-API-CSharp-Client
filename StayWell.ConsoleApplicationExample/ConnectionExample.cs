using System;
using System.Configuration;
using StayWell.Client;

namespace StayWell.ConsoleApplicationExample
{
    /// <summary>
    /// This example is showing how to connect to the StayWell API
    /// </summary>
    public class ConnectionExample
    {
        /// <summary>
        /// Method shows how to connect to the API
        /// </summary>
        public static ApiClient Connect()
        {
            //In this method we are passing in the ApplicationId and ApplicationSecret
            //To get your ApplicationId and ApplicationSecret talk to your Account Manager
            var client = new ApiClient(ConfigurationManager.AppSettings["ApplicationId"], SimpleContrivedEncryptionClass.DecryptString(ConfigurationManager.AppSettings["ApplicationSecret"]));
            Console.WriteLine("Connected to the API at this url : {0}", client.ServiceUri);
            return client;
        }

        /// <summary>
        /// Shows how to connect to the API at a different URL
        /// Using this method to connect to the API should be rare. The client has the default URL of https://api.kramesstaywell.com built in.
        /// </summary>
        /// <param name="url">URL that you use to connect to the StayWell API</param>
        public static ApiClient Connect(string url)
        {
            var client = new ApiClient(url, ConfigurationManager.AppSettings["ApplicationId"], SimpleContrivedEncryptionClass.DecryptString(ConfigurationManager.AppSettings["ApplicationSecret"]));
            Console.WriteLine("Connected to the API at this url : {0}", client.ServiceUri);
            return client;
        }
    }
}