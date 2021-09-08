using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos.Exceptions
{
    internal sealed class CosmosInvalidQueryRequestException : CosmosQueryException
    {

        public CosmosInvalidQueryRequestException(string message, string source = null) 
            : base(message)
        {
            base.Title = "Invalid Request";
            base.HResult = CosmosErrorCode.InvalidRequest;
            base.Source = source;
        }
    }
}
