using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos.Exceptions
{
    internal sealed class CosmosInvalidMethodChainException : CosmosQueryException
    {
        private const string message = "The query request object was invalid due to improper method chaining.";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methods">The effected methods trying to be set within the same instance.</param>
        public CosmosInvalidMethodChainException(string[] methods) 
            : base(message)
        {
            base.Title = "Invalid Method Chaining";
            base.HResult = CosmosErrorCode.InvalidMethodChaining;
            base.Source = $"The following functions cannot live in the same nested object:" +
                $" {string.Join(',', methods).Replace("Cosmos","$",StringComparison.InvariantCultureIgnoreCase)}.";
        }
    }
}
