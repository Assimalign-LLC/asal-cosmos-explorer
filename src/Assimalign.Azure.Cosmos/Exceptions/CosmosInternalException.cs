using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos.Exceptions
{
    internal class CosmosInternalException : CosmosQueryException
    {

        public CosmosInternalException(string message, Exception exception) 
            : base(message, exception)
        {
            base.HResult = CosmosErrorCode.Internal;
        }

        public CosmosInternalException(string message, string source = null) : base(message)
        {
            base.HResult = CosmosErrorCode.Internal;
            base.Source = source;
        }
        
    }
}
