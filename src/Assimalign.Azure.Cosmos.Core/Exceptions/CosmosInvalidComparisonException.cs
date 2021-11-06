using System;
using System.Collections.Generic;
using System.Text;

namespace Assimalign.Azure.Cosmos.Exceptions
{
    internal sealed class CosmosInvalidComparisonException : CosmosQueryException
    {
        private const string title = "Invalid Comparison";

        private const string message = "Member or Method Type does not match evaluation type.";
        private const string message1 = "Member or Method Type does not match evaluation type at {0} for member {1}";
        private const string message2 = "Member is either of a complex type of unsupported type that does not allow comparison. Member: {0}";


        public CosmosInvalidComparisonException(string message) 
            : base(message)
        {
            base.Title = title;
            base.HResult = CosmosErrorCode.InvalidComparison;
        }

        public CosmosInvalidComparisonException(string source, string member)
            : base(string.Format(message1, source, member))
        {
            base.Title = title;
            base.HResult = CosmosErrorCode.InvalidComparison;
        }

        public CosmosInvalidComparisonException(InvalidCastException exception) 
            : base (message, exception)
        {
            base.Title = title;
            base.HResult = CosmosErrorCode.InvalidComparison;
        }

        public CosmosInvalidComparisonException(Exception exception, string type)
            : base(exception.Message)
        {
            base.Title = title;
            base.HResult = CosmosErrorCode.InvalidComparison;
        }

    }
}
