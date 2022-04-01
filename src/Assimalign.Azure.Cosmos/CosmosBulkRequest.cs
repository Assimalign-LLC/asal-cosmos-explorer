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
    public class CosmosBulkRequest<T>
        where T : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<CosmosBulkRequestAction<T>> Actions { get; set; }
    }
}
