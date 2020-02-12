using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DatingApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private DataContext _context;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            if (_context.WeatherForecasts.Count() == 0) {
                var rng = new Random();
                var count = 0;
                WeatherForecast[] weathers = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Id = (count++).ToString(),
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
                foreach (var el in weathers) {
                    _context.Add(el);
                }
                _context.SaveChanges();
            }
            var result = await _context.WeatherForecasts.ToListAsync();
            
            return Ok(result);
        }
       
        [HttpGet("{id}")]
        public async Task<WeatherForecast> GetAsync(string id)
        {
            return await _context.WeatherForecasts.FindAsync(id);
        }
    }
}
