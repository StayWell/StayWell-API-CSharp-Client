using System;
using StayWell.Interface;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	public class AbbreviatedCollectionResponse
	{
		[Document("A unique identifier for the collection")]
		public Guid Id { get; set; }

		[Document("The title of the collection")]
		public string Title { get; set; }

		[Document("A human-readable unique identifier")]
		public string Slug { get; set; }
	}
}
