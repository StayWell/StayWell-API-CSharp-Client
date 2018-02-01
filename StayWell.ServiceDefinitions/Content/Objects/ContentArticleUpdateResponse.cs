using StayWell.ServiceDefinitions.Content.Interfaces;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	public class ContentArticleUpdateResponse : ContentArticleResponse, IContentMetaDataBaseUpdateResponse
	{
		public bool DraftUpdated { get; set; }
		public bool PublishedUpdated { get; set; }
	}
}