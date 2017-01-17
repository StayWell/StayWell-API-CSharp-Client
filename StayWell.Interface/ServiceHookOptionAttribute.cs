using System;

namespace StayWell.Interface
{
	[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = true)]
	public class ServiceHookOptionAttribute : Attribute
	{
		public string TypeString { get; set; }
		public string OptionName { get; set; }
		public object OptionValue { get; set; }

		public ServiceHookOptionAttribute(string typeString, string optionName, object optionValue)
		{
			TypeString = typeString;
			OptionName = optionName;
			OptionValue = optionValue;
		}
	}
}
