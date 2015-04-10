using System.Collections.Generic;
using System.Xml.Serialization;

namespace StayWell.Interface
{
	public class ResultList<T>
	{
		[XmlArrayItem("Item")]
		public List<T> Items { get; set; }
	}
}
