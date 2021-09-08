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

[assembly: WebJobsStartup(typeof(Assimalign.Azure.CosmosDemo.Startup))]
namespace Assimalign.Azure.CosmosDemo
{
    using Assimalign.Azure.Cosmos;
    using Assimalign.Azure.Cosmos.Bindings;
    using Assimalign.Azure.CosmosDemo.Http;
    

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
                    Connection = Environment.GetEnvironmentVariable("ConnectionStrings:AzureCosmosDbEmulator",EnvironmentVariableTarget.Process),
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
