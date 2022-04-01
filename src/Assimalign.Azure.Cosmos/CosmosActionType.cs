using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos
{
    public enum CosmosActionType : uint
    {
        Create = 1,
        Update = 2,
        Delete = 3,
        Patch = 4
    }
}
