using System;

namespace StayWell.Interface
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class DocumentAttribute : Attribute
	{
		public DocumentAttribute(string summary = null, string description = null, string tags = null)
		{
			Summary = summary;
			Description = description;
			Tags = tags;
		}

		public DocumentAttribute(UseType usage, string summary = null, string description = null, string tags = null)
		{
			Usage = usage;
			Summary = summary;
			Description = description == null ? null : string.Join(Environment.NewLine + Environment.NewLine, description);
			Tags = tags;
		}

		public string Name { get; set; }
		public string Summary { get; set; }
		public string Description { get; set; }
		public UseType Usage { get; set; }
		public AdvancedType AdvancedType { get; set; }
		public string Tags { get; set; }
	}
}
