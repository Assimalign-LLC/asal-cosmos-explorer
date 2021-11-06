using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos.Clauses
{
    public interface ICosmosSelectFilter : ICosmosField
    {
        OperatorType Operator { get; set; }
        ValueTypeFunctions Function { get; set; }
        object Value { get; set; }
    }



    public interface ICosmosSelectFilter<TFilter> : ICosmosSelectFilter
        where TFilter : ICosmosSelectFilter
    {
        IEnumerable<TFilter> Or { get; set; }

        IEnumerable<TFilter> And { get; set; }
    }
}
