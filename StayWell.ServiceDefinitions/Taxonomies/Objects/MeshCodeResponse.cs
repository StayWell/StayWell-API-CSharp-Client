using System.Collections.Generic;


namespace StayWell.ServiceDefinitions.Taxonomies.Objects
{
	public class MeshCodeResponse
	{
		public string Slug { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public List<string> Terms { get; set; }
		public bool Obsolete { get; set; }
	}
}
