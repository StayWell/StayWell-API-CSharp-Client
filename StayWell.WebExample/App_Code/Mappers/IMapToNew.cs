using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayWell.WebExample.App_Code.Mappers
{
    public interface IMapToNew<TSource, TTarget>
    {
        TTarget Map(TSource source);
    }

}
