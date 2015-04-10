using System.Runtime.Serialization;

namespace StayWell.Interface
{
	public class PagedRequest
	{
		[DataMember(Name = "$skip")]
		public int Offset { get; set; }

		[DataMember(Name = "$top")]
		public int Count { get; set; }
	}
}
