using System.Collections.Generic;
using System.Xml.Serialization;
using StayWell.ServiceDefinitions.Extensions;

namespace StayWell.ServiceDefinitions.Content.Objects
{
    [SwaggerResponseDescription(@"The XML Structure of the SpellingSuggestions is actually in the following format not the format in the example
<SpellingSuggestions>
        <SpellingSuggestion>
            <Key>acut</Key>
            <Value>
                <String>acuta</String>
                <String>acet</String>
                <String>act</String>
            </Value>
        </SpellingSuggestion>
    </SpellingSuggestions>
")]
    public class SearchSuggestionsResponse
	{
        [XmlArrayItem("SpellingSuggestion")]
		public Dictionary<string, List<string>> SpellingSuggestions { get; set; }
	    [XmlArrayItem("TypeAheadSuggestion")]
        public List<string> TypeAheadSuggestions { get; set; }
	}

	public class SpellingSuggestionsResponse
	{
		public string Suggestion { get; set; }
	}
}
