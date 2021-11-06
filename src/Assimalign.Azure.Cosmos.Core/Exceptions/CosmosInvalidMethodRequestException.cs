using System;
using System.Collections.Generic;
using System.Text;

namespace Assimalign.Azure.Cosmos.Exceptions
{
    internal sealed class CosmosInvalidMethodRequestException : CosmosQueryException
    {
        public CosmosInvalidMethodRequestException(string message, string source = null) 
            : base(message)
        {
            base.Title = "Invalid Method Request";
            base.HResult = CosmosErrorCode.InvalidMethodRequest;
            base.Source = source;
        }
    }
}
