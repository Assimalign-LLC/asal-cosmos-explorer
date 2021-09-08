using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CosmosError
    {
        /// <summary>
        /// The error code to categorize the exceptions thrown within the SDK.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// A unique name for the particular error thrown.
        /// </summary>
        public string  Title { get; set; }

        /// <summary>
        /// A descriptive message of the error.
        /// </summary>
        public string Message { get; set; }


        /// <summary>
        /// The source of the error or errors if any.
        /// </summary>
        public string Source { get; set; }
    }
}
