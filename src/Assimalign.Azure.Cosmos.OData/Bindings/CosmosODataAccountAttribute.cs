using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos.OData.Bindings
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class CosmosODataAccountAttribute : Attribute, IConnectionProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        public CosmosODataAccountAttribute(string account)
        {
            this.Account = account;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Account { get; set; }

        string IConnectionProvider.Connection 
        {
            get
            {
                return Account;
            } 
            set
            {
                Account = value;
            }
        }
    }
}
