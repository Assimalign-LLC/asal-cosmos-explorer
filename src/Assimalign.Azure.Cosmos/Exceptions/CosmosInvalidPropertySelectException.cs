using System;
using System.Collections.Generic;
using System.Text;

namespace Assimalign.Azure.Cosmos.Exceptions
{
    internal sealed class CosmosInvalidPropertySelectException : CosmosQueryException
    {
        private const string message = "Property '{0}' does not exist on type: {1}";

        public CosmosInvalidPropertySelectException(Type type, string propertyName) :
            base(string.Format(message, propertyName, type.Name))
        {
            base.Title = "Invalid Select";
            base.HResult = CosmosErrorCode.InvalidSelect;
        }
    }
}
