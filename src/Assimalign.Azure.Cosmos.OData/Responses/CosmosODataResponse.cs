using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Assimalign.Azure.Cosmos.OData.Responses
{
    public static class CosmosODataResponse
    {


        public static IActionResult Ok<T>(CosmosResponse<T> response)
            where T : class, new()
        {
            throw new NotImplementedException();
        }

        public static IActionResult Accepted<T>(CosmosResponse<T> response)
            where T : class, new()
        {
            throw new NotImplementedException();
        }

    }
}
