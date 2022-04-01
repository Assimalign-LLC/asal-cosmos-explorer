using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos
{
    public class CosmosBulkTransaction<T>
        where T : class, new()
    {
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public T Item { get; set; }
    }
}
