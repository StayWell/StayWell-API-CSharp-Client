using System.Collections.Generic;
using System.Reflection;

namespace StayWell.Interface
{
    public interface IServiceHook
    {
        void Process(MethodInfo serviceMethodInfo, Dictionary<string, object> parameterList, Dictionary<string, object> serviceHookOptions);
    }
}
