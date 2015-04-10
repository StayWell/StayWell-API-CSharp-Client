using System;
using System.Linq;
using System.Reflection;
using System.Web;
using StayWell.ClientFramework.Interfaces;
using StayWell.ClientFramework.Objects;

namespace StayWell.ClientFramework.TokenStores
{
	public class CookieTokenStore : ITokenStore
	{
		private const string COOKIE_NAME = "KswApi.Token";

		public AccessToken GetToken(string applicationId)
		{
			if (HttpContext.Current == null)
				throw GetExceptionForInvalidContext();

            AccessToken token = (AccessToken)HttpContext.Current.Items[COOKIE_NAME];

			if (token == null)
			{
                if (HttpContext.Current.Request.Cookies.AllKeys.All(item => item != COOKIE_NAME))
					return null;

                HttpCookie cookie = HttpContext.Current.Request.Cookies[COOKIE_NAME];

				if (cookie == null)
					return null;

				token = DeserializeFromCookie(cookie);

				if (token == null)
					return null;
			}

			return token;
		}

		public void SetToken(AccessToken value, string applicationId)
		{
			if (HttpContext.Current == null)
				throw GetExceptionForInvalidContext();

            HttpContext.Current.Items[COOKIE_NAME] = value;

            HttpCookie cookie = new HttpCookie(COOKIE_NAME) { HttpOnly = true };

			SerializeToCookie(value, cookie);

			HttpContext.Current.Response.Cookies.Add(cookie);
		}

        public void RemoveToken(string applicationId)
		{
			if (HttpContext.Current == null)
				throw GetExceptionForInvalidContext();

            HttpContext.Current.Items.Remove(COOKIE_NAME);
            HttpContext.Current.Request.Cookies.Remove(COOKIE_NAME);

            HttpCookie expiredCookie = new HttpCookie(COOKIE_NAME) { HttpOnly = true, Expires = DateTime.UtcNow.AddDays(-1) };
		    HttpContext.Current.Response.Cookies.Add(expiredCookie);
		}

		private void SerializeToCookie(AccessToken token, HttpCookie cookie)
		{
			PropertyInfo[] properties = typeof(AccessToken).GetProperties(BindingFlags.Public | BindingFlags.Instance);
			foreach (PropertyInfo property in properties)
			{
				object value = property.GetValue(token, null);
				if (value != null)
					cookie[property.Name] = value.ToString();
			}
		}

		private AccessToken DeserializeFromCookie(HttpCookie cookie)
		{
			if (cookie == null)
				return null;

			PropertyInfo[] properties = typeof(AccessToken).GetProperties(BindingFlags.Public | BindingFlags.Instance);

			AccessToken token = new AccessToken();

			foreach (PropertyInfo property in properties)
			{
				object value = cookie.Values[property.Name];

				if (value == null)
					continue;

				if (property.PropertyType != typeof(string))
				{
					if (!property.PropertyType.IsEnum)
					{
						value = Convert.ChangeType(value, property.PropertyType);
					}
					else
					{
						if (Enum.IsDefined(property.PropertyType, value))
							value = Enum.Parse(property.PropertyType, (string) value, true);
					}
				}

				property.SetValue(token, value, null);
			}

			if (string.IsNullOrEmpty(token.Token))
				return null;

			return token;
		}

		private Exception GetExceptionForInvalidContext()
		{
			return new InvalidOperationException("A client with a cookie token store can only be used with an active request context.");
		}
	}
}
