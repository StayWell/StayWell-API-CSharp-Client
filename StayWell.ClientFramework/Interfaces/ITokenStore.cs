using StayWell.ClientFramework.Objects;

namespace StayWell.ClientFramework.Interfaces
{
	public interface ITokenStore
	{
		AccessToken GetToken(string applicationId);
        void SetToken(AccessToken value, string applicationId);
        void RemoveToken(string applicationId);
	}
}
