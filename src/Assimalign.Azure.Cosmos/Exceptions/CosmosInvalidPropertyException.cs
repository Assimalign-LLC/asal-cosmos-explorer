using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos.Exceptions
{
    internal sealed class CosmosInvalidPropertyException : CosmosQueryException
    {
        private const string title = "Invalid Property";

        public CosmosInvalidPropertyException(string message, string source = null) 
            : base(message)
        {
            base.Title = title;
            base.HResult = CosmosErrorCode.InvalidProperty;
            base.Source = source;
           
        }

        public CosmosInvalidPropertyException(string message, Exception innerException) 
            : base(message, innerException)
        {
            base.Title = title;
            base.HResult = CosmosErrorCode.InvalidProperty;
        }
    }
}
