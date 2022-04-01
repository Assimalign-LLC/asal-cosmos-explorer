using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos.Utilities
{
    internal static class CosmosEnumerableExtensions
    {

        /// <summary>
		/// 
		/// </summary>
		/// <param name="values"></param>
		/// <param name="action"></param>
		public static void ForEach(this IEnumerable<string> values, Action<string> action)
        {
            foreach (var value in values)
            {
                action(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IEnumerable<string> ForEach(this IEnumerable<string> values, Func<string, string> action)
        {
            var items = new List<string>();
            foreach (var value in values)
            {
                items.Add(action(value));
            }
            return items;
        }

    }
}
