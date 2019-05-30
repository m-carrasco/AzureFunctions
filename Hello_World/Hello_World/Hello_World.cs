using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics.Contracts;

namespace Hello_World
{
    public class Named
    {
        public string name;
    }

    public static class Hello_World
    {
        /*[FunctionName("Hello_World")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }*/
                
        public static IActionResult Run_NoBugs(
        //public static async Task<IActionResult> IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            // I'm removing await and dynamic typing.
            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            name = name ?? data?.name;

            // Refactoring a little so we can write an assertion for the result
            //return name != null
            //    ? (ActionResult)new OkObjectResult($"Hello, {name}")
            //    : new BadRequestObjectResult("Please pass a name on the query string or in the request body");



            ActionResult result = name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");


            Contract.Assert(result is OkObjectResult || result is BadRequestObjectResult);
            return result;
        }
        public static IActionResult No_Dynamic(
        //public static async Task<IActionResult> IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            // I'm removing await and dynamic typing.
            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            Named data = JsonConvert.DeserializeObject<Named>(requestBody);

            name = name ?? data.name;

            // Refactoring a little so we can write an assertion for the result
            //return name != null
            //    ? (ActionResult)new OkObjectResult($"Hello, {name}")
            //    : new BadRequestObjectResult("Please pass a name on the query string or in the request body");



            ActionResult result = name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");


            Contract.Assert(result is OkObjectResult || result is BadRequestObjectResult);
            Contract.Assert(result is OkObjectResult || name == null);
            return result;
        }

        public static IActionResult Run_Bugged(
        //public static async Task<IActionResult> IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            // I'm removing await and dynamic typing.
            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;
            
            // Refactoring a little so we can write an assertion for the result
            //return name != null
            //    ? (ActionResult)new OkObjectResult($"Hello, {name}")
            //    : new BadRequestObjectResult("Please pass a name on the query string or in the request body");



            ActionResult result = name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");


            Contract.Assert(!(result is OkObjectResult || result is BadRequestObjectResult));
            return result;
        }

        // corral output is no bugs
        public static IActionResult CreateObject_NoBugs(bool b)
        {
            ActionResult obj = b ? (ActionResult)new OkObjectResult("") : new BadRequestObjectResult("");

            Contract.Assert(obj is OkObjectResult || obj is BadRequestObjectResult); 
            return obj;
        }

        // corral output is assertion fails (which is right)
        public static IActionResult CreateObject_Bugged(bool b)
        {
            ActionResult obj = b ? (ActionResult)new OkObjectResult("") : new BadRequestObjectResult("");

            Contract.Assert(!(obj is OkObjectResult || obj is BadRequestObjectResult));
            return obj;
        }

        // corral output is no bugs
        public static IActionResult CreateObject_NoBugs_1(bool b)
        {
            ActionResult obj = b ? (ActionResult)new OkObjectResult("") : new BadRequestObjectResult("");

            Contract.Assert(obj is Object);
            return obj;
        }

        // corral output is assertion fails
        public static IActionResult CreateObject_Bugged_1(bool b)
        {
            ActionResult obj = b ? (ActionResult)new OkObjectResult("") : new BadRequestObjectResult("");

            Contract.Assert(!(obj is Object));
            return obj;
        }
    }
}
