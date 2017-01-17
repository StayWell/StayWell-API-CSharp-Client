using System;

namespace StayWell.Interface
{
	[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = true)]
	public class ServiceHookAttribute : Attribute
	{
		public string TypeString { get; set; }

		/// <summary>
		/// Pass in a type to be instantiated that will have it's Process method called before the service method executes.
		/// </summary>
		/// <param name="typeString">Comma separated string with fully qualified class name and assembly name. Example: "Your.Namespace.TheClass, Your.Assembly"</param>
		public ServiceHookAttribute(string typeString)
		{
			if (string.IsNullOrWhiteSpace(typeString))
			{
				throw new ArgumentException("typeString is required", "typeString");
			}
			TypeString = typeString;
		}
	}
}
