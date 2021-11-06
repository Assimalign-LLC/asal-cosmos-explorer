using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CosmosErrorCode
    {
        /// <summary>
        /// Invalid property either does not exist on the type being queried or 
        /// is invalid for the operation being requested.
        /// </summary>
        public const int InvalidProperty = unchecked(4001);

        /// <summary>
        /// Invalid method usually occurs when placing two functions within the same
        /// object. Each function should be nested within each other.
        /// </summary>
        public const int InvalidMethodChaining = unchecked(4002);

        /// <summary>
        /// Invalid comparison usually occurs when the left and right 
        /// expression (variables) types do not match. Bad Comparison 
        /// Examples: string.Trim() == true, string.StartsWith('field', 'value') == 1.
        /// </summary>
        public const int InvalidComparison = unchecked(4003);

        /// <summary>
        /// Invalid Request can happen for a number of reason, but usually refers
        /// that the request was not being able to be processed.
        /// </summary>
        public const int InvalidRequest = unchecked(4004);

        /// <summary>
        /// Either the requested method is not supported or the 
        /// methods being chained together are of different types.
        /// </summary>
        public const int InvalidMethodRequest = unchecked(4005);

        /// <summary>
        /// Invalid partition key usually occurs when a type does not match partition 
        /// requirements. Only Boolean, String, and Double are supported as partition keys.
        /// </summary>
        public const int InvalidPartitionKey = unchecked(4006);

        /// <summary>
        /// 
        /// </summary>
        public const int InvalidSelect = unchecked(4007);
        
        /// <summary>
        /// The Comparison value is of the incorrect type or not supported 
        /// due to form.
        /// </summary>
        public const int InvalidValue = unchecked(4008);

        /// <summary>
        /// Refers to an unknown error. If this error is returned this may be a bug and should 
        /// be reported.
        /// </summary>
        public const int Internal = unchecked(5000);

        /// <summary>
        /// 
        /// </summary>
        internal static int[] Codes = new int[]
        {
            InvalidProperty,
            InvalidMethodChaining,
            InvalidComparison,
            InvalidRequest,
            InvalidMethodRequest,
            InvalidPartitionKey,
            InvalidSelect,
            InvalidValue
        };


        /// <summary>
        /// Evaluates whether a code is native to packages or not. Will be used mainly for 
        /// Exception HResult tests for build CosmosError collection
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        internal static bool IsCodeInternal(int code) => Codes.Contains(code);
    }
}
