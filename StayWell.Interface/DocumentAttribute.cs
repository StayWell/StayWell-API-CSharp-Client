using System;
using System.Collections.Generic;
using System.Linq;

namespace StayWell.Interface
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class DocumentAttribute : Attribute
	{
		public DocumentAttribute()
		{

		}

		public DocumentAttribute(string description, params string[] notes)
		{
			Description = description;
			Notes = notes == null ? new List<string>() : notes.ToList();
		}

		public DocumentAttribute(UseType usage, string description, params string[] notes)
		{
			Usage = usage;
			Description = description;
			Notes = notes == null ? new List<string>() : notes.ToList();
		}

		public DocumentAttribute(string name, string description, params string[] notes)
		{
			Name = name;
			Description = description;
			Notes = notes == null ? new List<string>() : notes.ToList();
		}

		public DocumentAttribute(string name, UseType usage, string description, params string[] notes)
		{
			Name = name;
			Usage = usage;
			Description = description;
			Notes = notes == null ? new List<string>() : notes.ToList();
		}

		public string Name { get; set; }
		public string Description { get; set; }
		public List<string> Notes { get; set; }
		public UseType Usage { get; set; }
		public AdvancedType AdvancedType { get; set; }
	}
}
