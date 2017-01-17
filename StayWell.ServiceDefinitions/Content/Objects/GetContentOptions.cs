using System;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	public class GetContentOptions : GetContentBase
	{
		public DateTime? Time { get; set; }
		public bool EditMode { get; set; }
	}
}