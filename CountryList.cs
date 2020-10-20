using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using JsonResponse;
using Helper;

namespace CountryList
{
    public static class CountryListPathfinder
    {
        [FunctionName("CountryList")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "countrylist/{country}")] 
            HttpRequest req, ILogger log, string country)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            string input = country.ToUpper();
            List<string> path = null;
            log.LogInformation("C# HTTP trigger function processed a request.");

            // This computes the path and catches the two main error cases
            try
            {
                path = CountrySearch.shortestPath(input);
            }
            catch(KeyNotFoundException)
            {
                log.LogInformation($"Key {input} not contained in the map.");
                return new BadRequestObjectResult($"The destination {input} is not contained in North America");
            }
            if(path.Count == 0)
            {
                log.LogInformation($"No path from USA to {input} found in the map.");
                return new BadRequestObjectResult($"No path from USA to {input} found in the map");
            }
            
            // format and return succesful response
            PathResponse okPathResponce = new PathResponse{Start = "USA", Destination = input, Path = path};
            var okJsonResponse = JsonConvert.SerializeObject(okPathResponce);
            return new JsonResult(okPathResponce);
        }
    }
}

/*
            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(country)
                ? $"TEST {shortestPath(country)} TEST This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {}. This HTTP triggered function executed successfully.";


*/