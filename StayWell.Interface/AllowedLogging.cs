namespace StayWell.Interface
{
	public enum AllowedLogging
	{
		Log,
		NeverLog,
		LogWithoutParametersOrBody,
		LogWithoutBody,
		LogException,
		LogExceptionWithoutBody
	}
}
