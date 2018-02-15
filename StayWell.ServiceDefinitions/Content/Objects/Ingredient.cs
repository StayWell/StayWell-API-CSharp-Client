namespace StayWell.ServiceDefinitions.Content.Objects
{
	public class Ingredient
	{
		public string Name { get; set; }
		public string Quantity { get; set; } // create quantity class with fractional parts?
		public string Unit { get; set; }
		public string Note { get; set; }
	}
}
