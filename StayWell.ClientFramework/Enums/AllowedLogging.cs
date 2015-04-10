namespace KswApi.Interface.Enums
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
