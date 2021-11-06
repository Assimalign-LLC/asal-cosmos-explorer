using Assimalign.Azure.Cosmos.Functions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Assimalign.Azure.Cosmos.Functions
{

    sealed class CosmosDateTimePart : ICosmosFunction
    {
        public string Property { get; set; }

        public string Part { get; set; }


       

        public bool TryGetExpression(Expression parameter, out Expression expression)
        {
            throw new NotImplementedException();
        }
    }
}
