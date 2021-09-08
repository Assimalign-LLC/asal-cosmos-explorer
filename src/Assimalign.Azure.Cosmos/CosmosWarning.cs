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
    public class CosmosWarning
    {
        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The warning severity of the message.
        /// </summary>
        public CosmosWarningSeverity Severity { get; set; } = CosmosWarningSeverity.Low;

        /// <summary>
        /// A descriptive message of the warning.
        /// </summary>
        public string Message { get; set; }
    }
}
