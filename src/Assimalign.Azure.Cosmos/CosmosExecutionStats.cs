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
    public class CosmosExecutionStats
    {
        /// <summary>
        /// 
        /// </summary>
        public long EllapsedMilliseconds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long EllapsedSeconds => EllapsedMilliseconds / 1000;

        /// <summary>
        /// 
        /// </summary>
        public double EllapsedMinutes => EllapsedSeconds / 60;
    }
}
