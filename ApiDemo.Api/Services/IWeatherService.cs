using ApiDemo.Api.Domain;

namespace ApiDemo.Api.Services;

public interface IWeatherService
{
    IEnumerable<WeatherForecast> GetAll();
}