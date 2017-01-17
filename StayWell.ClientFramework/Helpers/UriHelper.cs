using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace StayWell.ClientFramework.Helpers
{
	internal static class UriHelper
	{
		public static bool HasScheme(string url)
		{
			return url.StartsWith(Uri.UriSchemeHttp, StringComparison.InvariantCultureIgnoreCase)
				   || url.StartsWith(Uri.UriSchemeHttps, StringComparison.InvariantCultureIgnoreCase);
		}

		public static string AddHttpScheme(string url)
		{
			return Uri.UriSchemeHttp + "://" + url;
		}

		public static string AddHttpsScheme(string url)
		{
			return Uri.UriSchemeHttps + "://" + url;
		}

		public static string Combine(params string[] parts)
		{
			StringBuilder builder = new StringBuilder();

			bool first = true;

			for (int i = 0; i < parts.Length; i++)
			{
				string current = parts[i];
				if (i != 0)
					current = current.TrimStart('/');
				if (i + 1 < parts.Length)
					current = current.TrimEnd('/');

				if (first)
					first = false;
				else
					builder.Append('/');

				builder.Append(current);
			}

			return builder.ToString();
		}

		public static string AddQuery(string baseUrl, NameValueCollection parameters)
		{
			if (parameters.Count == 0)
				return baseUrl;

			StringBuilder builder = new StringBuilder();

			builder.Append(baseUrl);

			bool first = false;

			if (!baseUrl.Contains("?"))
			{
				builder.Append('?');
				first = true;
			}

			foreach (string key in parameters.AllKeys)
			{
				if (first)
					first = false;
				else
					builder.Append('&');

				builder.Append(Uri.EscapeDataString(key));
				builder.Append('=');
				builder.Append(Uri.EscapeDataString(parameters[key] ?? string.Empty));
			}

			return builder.ToString();
		}

		public static string CreateQuery(NameValueCollection parameters)
		{
			StringBuilder builder = new StringBuilder();

			bool first = true;

			foreach (string key in parameters.AllKeys)
			{
				if (first)
					first = false;
				else
					builder.Append('&');

				builder.Append(Uri.EscapeDataString(key));
				builder.Append('=');
				builder.Append(Uri.EscapeDataString(parameters[key]));
			}

			return builder.ToString();
		}
	}
}
