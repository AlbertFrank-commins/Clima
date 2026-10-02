using System.Net.Http.Json;
using Clima.Models;

namespace Clima.Services;

public class WeatherService
{
    private readonly HttpClient _httpClient;

    public WeatherService()
    {
        _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };
    }

    public async Task<WeatherData> GetWeatherByCoordinatesAsync(string cityName, double latitude, double longitude)
    {
        // Endpoint público de Open-Meteo (sin API key)
        string url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current=temperature_2m,relative_humidity_2m,weather_code,wind_speed_10m";

        try
        {
            var response = await _httpClient.GetFromJsonAsync<OpenMeteoResponse>(url);

            if (response?.Current is null)
                throw new Exception("Respuesta vacía del satélite.");

            var current = response.Current;
            var (condition, icon, bgColor) = MapWeatherCode(current.WeatherCode);

            return new WeatherData
            {
                City = cityName,
                Temperature = $"{Math.Round(current.Temperature)}°C",
                Condition = condition,
                Icon = icon,
                Humidity = $"{current.RelativeHumidity}%",
                WindSpeed = $"{current.WindSpeed:0.#} km/h",
                BackgroundColor = bgColor
            };
        }
        catch
        {
            // Fallback en caso de no tener internet o error de conexión
            return new WeatherData
            {
                City = cityName,
                Temperature = "--°C",
                Condition = "Sin conexión",
                Icon = "📡",
                Humidity = "--%",
                WindSpeed = "-- km/h",
                BackgroundColor = Color.FromArgb("#4A5568")
            };
        }
    }

    // Traduce el WMO Weather Code a icono, texto y color
    private static (string Condition, string Icon, Color BackgroundColor) MapWeatherCode(int code)
    {
        return code switch
        {
            0 => ("Cielo despejado", "☀️", Color.FromArgb("#E67E22")),
            1 or 2 => ("Parcialmente nublado", "⛅", Color.FromArgb("#2C3E50")),
            3 => ("Nublado", "☁️", Color.FromArgb("#34495E")),
            45 or 48 => ("Neblina", "🌫️", Color.FromArgb("#5D6D7E")),
            51 or 53 or 55 => ("Llovizna", "🌦️", Color.FromArgb("#2980B9")),
            61 or 63 or 65 => ("Lluvia", "🌧️", Color.FromArgb("#1F3A60")),
            80 or 81 or 82 => ("Aguaceros", "⛈️", Color.FromArgb("#1A252F")),
            95 or 96 or 99 => ("Tormenta eléctrica", "⚡", Color.FromArgb("#17202A")),
            _ => ("Clima variable", "🌡️", Color.FromArgb("#2C3E50"))
        };
    }
}