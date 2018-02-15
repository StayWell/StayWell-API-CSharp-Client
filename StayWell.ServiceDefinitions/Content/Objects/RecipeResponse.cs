using System.Collections.Generic;
using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	public class RecipeResponse : ContentMetadataResponse
	{
		public string Copyright { get; set; }
		public string ImageUri { get; set; }
		public string ImageBucketSlug { get; set; }
		public string ImageSlug { get; set; }
		
		public string ProviderRecipeId { get; set; }
		public string Provider { get; set; } 
		public string ServingSize { get; set; } // Core?
		public float Servings { get; set; }
		public string Yield { get; set; } // is this really needed?

		public List<IngredientSection> IngredientSections { get; set; }
		[XmlArrayItem("Step")]
		public List<string> Steps { get; set; }
		[XmlArrayItem("Tip")]
		public List<string> Tips { get; set; }
		public List<RecipeCategory> Categories { get; set; }
		public NutritionInfo NutritionInfo { get; set; }
		public float TotalTimeMinutes { get; set; }
		public string TotalTimeDescription { get; set; } // keep?
		public float ActiveTimeMinutes { get; set; }
		public string ActiveTimeDescription { get; set; } // keep?
		public string MakeAheadTip { get; set; }
		public List<string> Disclaimers { get; set; }
		public List<string> Equipment { get; set; }
		public string MainIngredient { get; set; }
	}
}
