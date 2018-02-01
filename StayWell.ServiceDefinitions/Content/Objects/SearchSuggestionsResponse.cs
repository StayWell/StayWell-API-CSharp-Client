using System.Collections.Generic;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	public class SearchSuggestionsResponse
	{
		public Dictionary<string, List<string>> SpellingSuggestions { get; set; }
		public List<string> TypeAheadSuggestions { get; set; }
	}

	public class SpellingSuggestionsResponse
	{
		public string Suggestion { get; set; }
	}
}
