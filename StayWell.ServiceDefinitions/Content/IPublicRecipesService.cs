using System.ServiceModel;
using System.ServiceModel.Web;
using StayWell.Interface;
using StayWell.ServiceDefinitions.Content.Objects;

namespace StayWell.ServiceDefinitions.Content
{
	[ServiceContract(Name = "Recipes", Namespace = "http://www.kramesstaywell.com")]
	public interface IPublicRecipesService
	{
		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{idOrSlug}", Method = "GET")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content", SpecialAccess = AllowedSpecialAccess.Jsonp)]
		[Document("Get the metadata for a streaming media resource")]
		RecipeResponse GetRecipe(string bucketIdOrSlug, string idOrSlug, GetContentOptions options);

		[WebInvoke(UriTemplate = "{bucketIdOrSlug}/{idOrSlug}/Rating", Method = "POST")]
		[Allow(ClientType = ClientType.Internal, Rights = "Read_Content")]
		[Document("Add a rating for a streaming media resource")]
		ContentRatingResponse AddRating(string bucketIdOrSlug, string idOrSlug, RatingRequest theRequest);
	}
}
