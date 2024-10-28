using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net;

namespace MyCutomFunction
{
    public class Function1
    {
        private readonly ILogger _logger;

        public Function1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
        }

        [Function("Function1")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var response = req.CreateResponse(HttpStatusCode.OK);
          
            var arr = req.Url.Query.TrimStart('?').Split("&",StringSplitOptions.RemoveEmptyEntries).ToList();
            NameValueCollection nameValue = new();
            if (arr.Any())
            {
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                arr.ForEach(item =>
                  {
                      var name = item.Split("=");
                      nameValue.Add(name[0], name[1]);
                  });
                response.WriteString("Welcome to Azure Functions!" + nameValue.Get("name"));
            }
            else
            {
                response.WriteAsJsonAsync(req.Body, "application/json; charset=utf-8");
            }
            
            return response;
        }
    }
}
