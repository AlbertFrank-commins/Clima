using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Clima.Models;
using Clima.Services;

namespace Clima.ViewModels;

public class WeatherViewModel : INotifyPropertyChanged
{
    private readonly WeatherService _weatherService = new();
    private int _cityIndex = 0;

    // Ciudades con sus coordenadas reales (latitud, longitud)
    private readonly List<(string City, double Lat, double Lon)> _cities = new()
    {
        ("Santo Domingo", 18.4861, -69.9312),
        ("La Romana", 18.4273, -68.9728),
        ("Punta Cana", 18.5820, -68.4055),
        ("Santiago de los Caballeros", 19.4517, -70.6970),
        ("Constanza", 18.9092, -70.7449)
    };

    private WeatherData? _currentWeather;
    public WeatherData? CurrentWeather
    {
        get => _currentWeather;
        set => SetProperty(ref _currentWeather, value);
    }

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    private string _loadingMessage = "Consultando Open-Meteo...";
    public string LoadingMessage
    {
        get => _loadingMessage;
        set => SetProperty(ref _loadingMessage, value);
    }

    public ICommand RefreshWeatherCommand { get; }

    public WeatherViewModel()
    {
        RefreshWeatherCommand = new Command(async () => await RefreshWeatherAsync());

        // Carga inicial al arrancar
        Task.Run(async () =>
        {
            var initial = _cities[0];
            CurrentWeather = await _weatherService.GetWeatherByCoordinatesAsync(initial.City, initial.Lat, initial.Lon);
        });
    }

    public async Task RefreshWeatherAsync()
    {
        if (IsLoading) return;

        IsLoading = true;
        LoadingMessage = "Obteniendo datos satelitales en vivo...";

        _cityIndex = (_cityIndex + 1) % _cities.Count;
        var target = _cities[_cityIndex];

        // Consulta HTTP real a la API
        CurrentWeather = await _weatherService.GetWeatherByCoordinatesAsync(target.City, target.Lat, target.Lon);

        IsLoading = false;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value)) return;
        storage = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}