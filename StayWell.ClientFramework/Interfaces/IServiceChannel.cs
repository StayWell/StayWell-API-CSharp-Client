using StayWell.ClientFramework.Internal;

namespace StayWell.ClientFramework.Interfaces
{
	internal interface IServiceChannel
	{
		object Invoke(OperationRequest operation);
	}
}
