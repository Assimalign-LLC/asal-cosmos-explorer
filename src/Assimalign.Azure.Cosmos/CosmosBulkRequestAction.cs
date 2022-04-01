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
    /// <typeparam name="T"></typeparam>
    public class CosmosBulkRequestAction<T>
        where T : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        public CosmosActionType ActionType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public T Item { get; set; }
    }
}
