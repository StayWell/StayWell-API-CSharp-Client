using System;

namespace StayWell.Interface
{
	public class AllowAttribute : Attribute
	{
		public AllowAttribute()
		{
			// default to not logging, only log explicitly specified logging
			Logging = AllowedLogging.NeverLog;
			Documentation = true;
		}

		public ClientType ClientType { get; set; }
		public string Rights { get; set; }
		public AllowedLogging Logging { get; set; }
		public AllowedSpecialAccess SpecialAccess { get; set; }
		public bool Documentation { get; set; }
		public bool AllowDuringInitialization { get; set; }

		public OperationReturnModifier ReturnModifier { get; set; }

		// currently extensions is information only, it doesn't actually modify the operation
		public string Extensions { get; set; }
	}
}
