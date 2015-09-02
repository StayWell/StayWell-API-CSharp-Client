using StayWell.WebExample.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml.Serialization;

namespace StayWell.WebExample.App_Code
{
    public static class ContentCategoryFactory
    {
        #region Factory

        static public ContentCategoryList GetRelatedServicesCategories()
        {
            const string PATH = "/ContentConfig/RelatedServicesCategories.config";
            return GetContentCategoryList(PATH);
        }

        static public ContentCategoryList GetVideoLibraryCategories()
        {
            const string PATH = "/ContentConfig/VideoLibraryCategories.config";
            return GetContentCategoryList(PATH);
        }

        static private ContentCategoryList GetContentCategoryList(string path)
        {
            StreamReader streamReader = null;
            ContentCategoryList categoryList;
            try
            {
                if (HttpContext.Current.Cache[path] != null) categoryList = (ContentCategoryList)HttpContext.Current.Cache[path];
                else
                {
                    path = HttpContext.Current.Server.MapPath(path);
                    streamReader = new StreamReader(path);

                    XmlSerializer serializer = new XmlSerializer(typeof(ContentCategoryList));
                    categoryList = (ContentCategoryList)serializer.Deserialize(streamReader);

                    CacheDependency configDependency = new CacheDependency(path);
                    HttpContext.Current.Cache.Add(path, categoryList, configDependency, Cache.NoAbsoluteExpiration, new TimeSpan(24, 0, 0), CacheItemPriority.High, null);
                }
            }
            finally
            {
                if (streamReader != null) streamReader.Close();
            }

            return categoryList;
        }
        #endregion
    }
    
    [XmlRootAttribute("ContentCategoryList", Namespace = "", IsNullable = false)]
    public class ContentCategoryList
    {
        public ContentCategoryList()
        {
            Categories = new List<Category>();
        }

        [XmlArray]
        public List<Category> Categories {get; set;}
    }
}