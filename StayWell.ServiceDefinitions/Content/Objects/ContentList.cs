using System.Collections.Generic;
using System.Xml.Serialization;
using StayWell.Interface;
using StayWell.ServiceDefinitions.Extensions;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	[XmlType("Content")]
    [SwaggerResponseDescription(@" The XML Structure of the SpellingSuggestions is actually in the following format not the format in the example
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

The Xml format of the TypeCounts node will follow this example not the example below:   <TypeCounts>
        <Types>
            <Type>
                <Key>Bmp</Key>
                <Value>3</Value>
            </Type>
            <Type>
                <Key>Gif</Key>
                <Value>2</Value>
            </Type>
        </Types>
        <Other>0</Other>
        <Missing>0</Missing>
        <Total>6</Total>
    </TypeCounts>")]
	public class ContentList : PagedResultList<ContentResponse>
	{
		public TypeCount TypeCounts { get; set; }
		public bool IncludeBlockedContent { get; set; }

	    [XmlArrayItem("SpellingSuggestion")]
	    public Dictionary<string, List<string>> SpellingSuggestions { get; set; }
    }
}
