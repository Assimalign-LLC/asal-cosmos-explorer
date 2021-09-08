using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos.Clauses
{
    /// <summary>
    /// Represents reserved functions that have no parameters.
    /// </summary>
    public enum ValueTypeFunctions
    {
        /// <summary>
        /// The default set for a filtering.
        /// </summary>
        None = 0,
        /// <summary>
        /// A reserved String Function to turn all characters to Lower Case.
        /// </summary>
        ToLower = 1,
        /// <summary>
        /// A reserved String Function to turn all characters to Upper Case.
        /// </summary>
        ToUpper = 2,
        //Ceiling = 3,
        //Floor = 4,
        //Round = 5
    }
}
