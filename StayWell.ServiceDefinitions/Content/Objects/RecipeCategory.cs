using System.Collections.Generic;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	public class RecipeCategory
	{
		public string CategoryType { get; set; } // make an enum?
		public List<string> Categories { get; set; }
	}
}
