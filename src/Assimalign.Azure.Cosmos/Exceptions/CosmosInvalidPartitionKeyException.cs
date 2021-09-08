using System;
using System.Collections.Generic;
using System.Text;

namespace Assimalign.Azure.Cosmos.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class CosmosInvalidPartitionKeyException : CosmosQueryException
    {
        private const string message = "Partition Key type invalid. Excepted Partition Types are 'Boolean', 'String', and 'Double'";
        
        /// <summary>
        /// 
        /// </summary>
        public CosmosInvalidPartitionKeyException(string source = null)
            : base(message)
        {
            base.Title = "Invalid Partition Key";
            base.HResult = CosmosErrorCode.InvalidPartitionKey;
            base.Source = source;
        }
    }
}
