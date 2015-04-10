using System;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	public class ContentVersionItem
	{
		public Guid Id { get; set; }
		public string Slug { get; set; }
		public Language Language { get; set; }
		public bool Master { get; set; }
		public bool Current { get; set; }
	}
}
