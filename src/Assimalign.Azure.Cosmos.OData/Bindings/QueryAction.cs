using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos.OData.Bindings
{
    public enum QueryAction
    {
        Select = 1,
        Expand = 2,
        Filter = 3,
        OrderBy = 4,
        Search = 5,
    }
}
