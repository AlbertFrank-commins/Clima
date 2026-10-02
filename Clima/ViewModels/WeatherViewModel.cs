using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Clima.Models;

namespace Clima.ViewModels;

public class WeatherViewModel : INotifyPropertyChanged
{
    private int _cityIndex = 0;

    private readonly List<WeatherData> _mockData = new()
    {
        new WeatherData
        {
            City = "Santo Domingo",
            Temperature = "29°C",
            Condition = "Parcialmente Nublado",
            Icon = "⛅",
            Humidity = "78%",
            WindSpeed = "18 km/h",
            BackgroundColor = Color.FromArgb("#2C3E50")
        },
        new WeatherData
        {
            City = "La Romana",
            Temperature = "31°C",
            Condition = "Soleado y Caluroso",
            Icon = "☀️",
            Humidity = "65%",
            WindSpeed = "14 km/h",
            BackgroundColor = Color.FromArgb("#E67E22")
        },
        new WeatherData
        {
            City = "Constanza",
            Temperature = "16°C",
            Condition = "Lluvia Ligera",
            Icon = "🌧️",
            Humidity = "90%",
            WindSpeed = "8 km/h",
            BackgroundColor = Color.FromArgb("#34495E")
        }
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

    private string _loadingMessage = "Consultando satélite...";
    public string LoadingMessage
    {
        get => _loadingMessage;
        set => SetProperty(ref _loadingMessage, value);
    }

    public ICommand RefreshWeatherCommand { get; }

    public WeatherViewModel()
    {
        CurrentWeather = _mockData[0];
        RefreshWeatherCommand = new Command(async () => await RefreshWeatherAsync());
    }

    public async Task RefreshWeatherAsync()
    {
        if (IsLoading) return;

        IsLoading = true;
        LoadingMessage = "Sincronizando pronóstico...";

        // Simulación de latencia de red (2 segundos)
        await Task.Delay(2000);

        _cityIndex = (_cityIndex + 1) % _mockData.Count;
        CurrentWeather = _mockData[_cityIndex];

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