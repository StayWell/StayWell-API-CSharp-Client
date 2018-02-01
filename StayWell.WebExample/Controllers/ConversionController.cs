using StayWell.Client;
using StayWell.ServiceDefinitions.Content.Objects;
using StayWell.WebExample.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StayWell.WebExample.App_Code;

namespace StayWell.WebExample.Controllers
{
    public class ConversionController : Controller
    {
        private const int DEFAULT_COUNT = 50;

        //Create an authenticated SW API client
        private ApiClient _client = new ApiClient(ConfigurationManager.AppSettings["ApplicationId"], SimpleContrivedEncryptionClass.DecryptString(ConfigurationManager.AppSettings["ApplicationSecret"]));

        #region Public Controller Actions

        //
        // GET: /Conversion/DisplayContentByLegacyId
        public ActionResult DisplayContentByLegacyId()
        {
            ContentArticleResponse article = _client.Content.GetLegacyContent("85", "P00816", new LegacyContentOptions
            {
                IncludeBody = true
            });

            return View(article);
        }

        //
        //
        // GET: /Conversion/VerifyLegacyIds/
        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)] //Disable cache to ensure the file uploaded is the alwasys the latest.
        public ActionResult VerifyLegacyIds(string ApplicationKey, string ApplicationSecret)
        {
            if (Request != null)
            {
                //If values are provided we will override the default application key and secret from configuration.
                ApiClient localClient = _client;
                if (!String.IsNullOrEmpty(ApplicationKey) && !String.IsNullOrEmpty(ApplicationSecret))
                {
                    localClient = new ApiClient(ApplicationKey, ApplicationSecret);
                }

                LegacyContentValidationReportModel report = new LegacyContentValidationReportModel();
                
                //Process the uploaded file.
                HttpPostedFileBase file = Request.Files["UploadedFile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    StreamReader csvreader = new StreamReader(file.InputStream);
                    while (!csvreader.EndOfStream)
                    {
                        var line = csvreader.ReadLine();
                        string[] values = line.Split(',');
                        report.Items.Add(new LegacyContentModel
                        {
                             ContentTypeId = values[0],
                             ContentId = values[1]
                        });
                    }
                }

                //Test if content is available.
                foreach (LegacyContentModel item in report.Items)
                {
                    try
                    {
                        ContentArticleResponse article = localClient.Content.GetLegacyContent(item.ContentTypeId, item.ContentId, new LegacyContentOptions());
                        item.IsAvailable = true;
                    }
                    catch (Exception ex)
                    {
                        item.IsAvailable = false;
                        item.NotAvailableReason = ex.Message;
                    }
                }
                
                //Prepare the report
                report.ItemCount = report.Items.Count;
                report.ErrorCount = report.Items.Count(c => c.IsAvailable == false);
                
                return View(report);
            }

            return View();
        }
        #endregion
    }
}