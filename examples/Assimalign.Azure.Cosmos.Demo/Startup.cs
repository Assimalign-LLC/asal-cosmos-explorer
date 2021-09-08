using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Azure.Identity;

[assembly: WebJobsStartup(typeof(Assimalign.Azure.Cosmos.Demo.Startup))]
namespace Assimalign.Azure.Cosmos.Demo
{
    using Assimalign.Azure.Cosmos.Bindings;
    using Assimalign.Azure.Cosmos.Demo.Http;
    

    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddCosmosExtension<EmployeeQuery.Employee>(configure =>
            {
                return new CosmosOptions()
                {
                    Database = "ErpCore",
                    Container = "Employee",
                    Connection = "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                    ClientOptions = new CosmosClientOptions()
                    {
                        AllowBulkExecution = true,
                        SerializerOptions = new CosmosSerializationOptions()
                        {
                            IgnoreNullValues = true,
                            PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase,
                        }
                    }
                };
            });
        }

        public class AzureCosmosConfigurations
        {
            public string Uri { get; set; }
            public string Key{ get; set; }
            public string Connection { get; set; }
            public IEnumerable<CosmosDatabase> Databases { get; set; }


            public partial class CosmosDatabase
            {
                public string Database { get; set; }
                public string Container { get; set; }
            }

        }

       
    }
}
