namespace StayWell.ServiceDefinitions.Mapping.Objects
{
	public class MappingCountResponse
	{
		public string Slug { get; set; }
		public string Name { get; set; }
		public int Total { get; set; }
		public int Mapped { get; set; }
	}
}
