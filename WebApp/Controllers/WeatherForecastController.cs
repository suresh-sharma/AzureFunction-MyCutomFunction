using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Text;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly HttpClient _httpClient;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet("CallFunction")]
        public async Task<IActionResult> CallFunction(string name)
        {
            var functionUrl = "http://localhost:7278/api/Function1?name=" + name;
            var response = await _httpClient.GetAsync(functionUrl);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }

            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }
        [HttpGet("SaveData")]
        public async Task<IActionResult> SaveData()
        {
            var Data= Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
            var jsosnData = JsonConvert.SerializeObject(Data);
            var content=new StringContent(jsosnData,Encoding.UTF8,"application/json");
            var functionUrl = "http://localhost:7278/api/Function1";
            var response = await _httpClient.PostAsync(functionUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }

            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }
    }
}
