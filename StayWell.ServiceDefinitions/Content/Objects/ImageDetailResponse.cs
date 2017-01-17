using System.Collections.Generic;
using System.Xml.Serialization;
using StayWell.ServiceDefinitions.Common.Objects;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	[XmlType("Image")]
	public class ImageDetailResponse : BinaryDetailResponse
	{
		public int Width { get; set; }
		public int Height { get; set; }
		//public List<AgeCategory> AgeCategories { get; set; }
		public Ethnicity Ethnicity { get; set; }
		//public Gender Gender { get; set; }
		public bool? ForFamily { get; set; }
		public List<Language> AssociatedLanguages { get; set; }
        public string AlternateText { get; set; }
        public string Caption { get; set; }
	}
}
