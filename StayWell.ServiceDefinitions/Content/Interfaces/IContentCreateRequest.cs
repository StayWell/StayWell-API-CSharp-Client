namespace StayWell.ServiceDefinitions.Content.Interfaces
{
	public interface IContentCreateRequest : IContentRequest // <==== rename to IContentUpdateRequest?
	{
		string MasterId { get; set; }

		string LanguageCode { get; set; }

		string Slug { get; set; }
	}
}
