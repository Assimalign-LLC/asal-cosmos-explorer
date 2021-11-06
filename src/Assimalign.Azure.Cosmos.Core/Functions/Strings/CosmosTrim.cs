using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Assimalign.Azure.Cosmos.Functions
{
    using Assimalign.Azure.Cosmos.Utilities;
    using Assimalign.Azure.Cosmos.Exceptions;

    /// <summary>
    /// 
    /// </summary>
    public sealed class CosmosTrim : CosmosFunctions, ICosmosFunction
    {
        /// <summary>
        /// 
        /// </summary>
        public CosmosTrim() { }

        /// <summary>
        /// The Character to trim from the both the end and start of the String Value.
        /// </summary>
        [JsonPropertyName("trimchar")]
        [Obsolete("Not Currently Supported")]
        public char Character { get; set; } = ' ';


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool TryGetExpression(Expression parameter, out Expression expression)
        {
            throw new NotImplementedException();
        }
    }
}
