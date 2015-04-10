using System.Xml.Serialization;

namespace StayWell.Interface
{
	[XmlRoot("Slug")]
	public class SlugResponse
	{
		public string Value { get; set; }
	}
}
