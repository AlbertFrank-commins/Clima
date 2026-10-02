using System.Text.Json.Serialization;

namespace Clima.Models;

public class OpenMeteoResponse
{
    [JsonPropertyName("current")]
    public CurrentWeatherDto? Current { get; set; }
}

public class CurrentWeatherDto
{
    [JsonPropertyName("temperature_2m")]
    public double Temperature { get; set; }

    [JsonPropertyName("relative_humidity_2m")]
    public int RelativeHumidity { get; set; }

    [JsonPropertyName("wind_speed_10m")]
    public double WindSpeed { get; set; }

    [JsonPropertyName("weather_code")]
    public int WeatherCode { get; set; }
}