using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Assimalign.Azure.Cosmos.Demo.Http
{
    using Assimalign.Azure.Cosmos.Bindings;
    using Assimalign.Azure.Cosmos.Exceptions;
    

    public class EmployeeQuery
    {
        private readonly ICosmosRepository<Employee> repository;

        public EmployeeQuery(
            ICosmosRepository<Employee> repository)
        {
            this.repository = repository;
        }


        [FunctionName("HttpCosmosCommandCreateEmployee")]
        public async Task<IActionResult> PostEMployeeItemAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "command/employees")] Employee employee,
            ILogger logger)
        {
            try
            {
                var results = await repository.CreateItemAsync(employee, key=>key.YearStarted);

                return OkJsonResponse<CosmosResponse<Employee>>.Create(results, 201);
            }
            catch(CosmosQueryException exception)
            {
                return new BadRequestObjectResult(exception.GetErrors());
            }
            catch (Exception exception)
            {
                return new BadRequestResult();
            }
        }

        [FunctionName("HttpCosmosCreateEmployeesBatch")]
        public async Task<IActionResult> PostEmployeeItemsAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "command/employees/batch")] Employee[] employees,
            ILogger logger)
        {
            try
            {
                CosmosBulkResponse results = null;
                var partitions = employees.Select(s => s.YearStarted).Distinct();


                foreach(var partition in partitions)
                {
                    var items = employees.Where(x => x.YearStarted == partition);
                    results = await repository.CreateItemsAsync(items, partition);
                }
               
                return OkJsonResponse<CosmosBulkResponse>.Create(results, 201);

            }
            catch (Exception exception)
            {
                return new BadRequestResult();
            }
        }


        [FunctionName("HttpCosmosQueryEmployee")]
        public async Task<IActionResult> GetUsersAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "query/employees")] HttpRequest request,
            [CosmosQuery] ICosmosQuery<Employee> query,
            [CosmosQuery] ICosmosRepository<Employee> repository,
            ILogger logger)
        {
            try
            {

                repository.Container.GetItemQueryIterator()
                    .W
               
                var results = await repository.GetItemsAsync(query);

                return OkJsonResponse<CosmosResponse<Employee>>.Create(results, 200);
            }
            catch(CosmosQueryException exception)
            {
                return new BadRequestObjectResult(exception.GetErrors());
            }
            catch(Exception exception)
            {
                var type = exception.GetType();
                return new BadRequestObjectResult(exception);
            }
        }



        public class Employee
        {
            private string id;

            public string Id
            {
                get
                {
                    if (id is null)
                    {
                        id = Guid.NewGuid().ToString();
                    }
                    return id;
                }
                set => id = value;
            }
            public string YearStarted { get; set; } = DateTime.Now.Year.ToString();
            public bool? IsActive { get; set; }
            public Guid? EmployeeId
            {
                get
                {
                    if (Id is null)
                    {
                        return Guid.NewGuid();
                    }
                    else
                    {
                        return Guid.Parse(id);
                    }
                }
                set
                {
                    Id = value.ToString();
                }
            }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string MiddleName { get; set; }

            public string Title { get; set; }
            public string Manager { get; set; }
            public string Department { get; set; }
            public DateTime? Birthdate { get; set; }
            public EmployeeTaxInformation TaxInfo { get; set; }
            public IEnumerable<EmployeeAddress> Addresses { get; set; }
            public IList<string> Tags { get; set;  }
        }
        public class EmployeeTaxInformation
        {

            public string Ssn { get; set; }

            public decimal? Salary { get; set; }

            public PayPeriod period { get; set; }

            public IEnumerable<EmployeePayStub> PaymentStubs { get; set; }

        }

        public class EmployeePayStub
        {
            public decimal Amount { get; set; }
            public DateTime paymentDate { get; set; }

            
        }

        public enum PayPeriod
        {
            Weekly = 0,
            BiWeekly = 1,
            SemiMonthly = 2,
            Monthly = 3
        }


        public class EmployeeAddress
        {
            public string AddressType { get; set; }
            public string Street { get; set; }
            public string Apt { get; set; }
            public string Suite { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string PostalCode { get; set; }
            public string Country { get; set; }
        }
        public class OkJsonResponse<T> : IActionResult
        {

            public OkJsonResponse(T data, int statusCode)
            {
                Data = data;
                StatusCode = statusCode;
            }

            public T Data { get; set; }

            public int StatusCode { get; set; }

            public static OkJsonResponse<T> Create(T data, int statusCode) => new OkJsonResponse<T>(data, statusCode);


            public Task ExecuteResultAsync(ActionContext context)
            {
                return Task.Run(async () =>
                {
                    context.HttpContext.Response.StatusCode = StatusCode;
                    context.HttpContext.Response.ContentType = "application/vnd.api+json";

                    using(MemoryStream stream = new MemoryStream())
                    {
                        await JsonSerializer.SerializeAsync(stream, this.Data, new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true,
                            ReadCommentHandling = JsonCommentHandling.Skip,
                            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                            AllowTrailingCommas = true,
                            IgnoreNullValues = true
                        });

                        stream.Position = 0;

                        await stream.CopyToAsync(context.HttpContext.Response.Body);
                    }
                    var response = context.HttpContext.Response;

                });
            }
        }
    }
}
