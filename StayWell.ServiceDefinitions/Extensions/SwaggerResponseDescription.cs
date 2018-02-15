using System;
using System.Net;
using System.Web;

namespace StayWell.ServiceDefinitions.Extensions
{
    public static class SwaggerResponseDescription
    {
        public static string GetSwaggerResponseDescription(this Type description)
        {
            string result = "Successful Response";
            var value = (SwaggerResponseDescriptionAttribute)Attribute.GetCustomAttribute(description, typeof(SwaggerResponseDescriptionAttribute));
            if (value != null)
                result = WebUtility.HtmlEncode(value.Description);

            return result;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class SwaggerResponseDescriptionAttribute : Attribute
    {
        public SwaggerResponseDescriptionAttribute(string description)
        {
            Description = description;
        }
        public string Description { get;  set; }
    }
}
