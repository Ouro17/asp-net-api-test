using ApiDemo.Api.Domain;
using ApiDemo.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiDemo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
sealed public class WeatherController(IWeatherService weatherervice) : ControllerBase
{
    private readonly IWeatherService _weatherervice = weatherervice;

    [AllowAnonymous, HttpGet]
    public ActionResult<IEnumerable<WeatherForecast>> Get() => Ok(_weatherervice.GetAll());
}