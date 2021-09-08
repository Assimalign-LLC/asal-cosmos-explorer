using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos.Exceptions
{
    internal sealed class CosmosInvalidValueException : CosmosQueryException
    {

        public CosmosInvalidValueException(string message, string source = null) 
            : base(message)
        {
            base.Title = "Invalid Value";
            base.HResult = CosmosErrorCode.InvalidValue;
            base.Source = source;
        }
        
    }
}
