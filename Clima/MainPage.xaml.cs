using Clima.ViewModels;

namespace Clima;

public partial class MainPage : ContentPage
{
    private readonly WeatherViewModel _viewModel;
    private bool _isSpinning;

    public MainPage()
    {
        InitializeComponent();
        _viewModel = (WeatherViewModel)BindingContext;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Animación de entrada inicial: la tarjeta sube suavemente con Fade
        WeatherCard.Opacity = 0;
        WeatherCard.TranslationY = 40;

        await Task.WhenAll(
            WeatherCard.FadeTo(1, 600, Easing.CubicOut),
            WeatherCard.TranslateTo(0, 0, 600, Easing.CubicOut)
        );
    }

    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        if (_viewModel.IsLoading) return;

        // 1. Micro-interacción: rebote rápido al tocar el botón
        await RefreshButton.ScaleTo(0.92, 100, Easing.CubicOut);
        await RefreshButton.ScaleTo(1.0, 100, Easing.CubicIn);
        RefreshButton.IsEnabled = false;

        // 2. Salida animada de la tarjeta actual (se desvanece y sube)
        await Task.WhenAll(
            WeatherCard.FadeTo(0, 250, Easing.CubicIn),
            WeatherCard.TranslateTo(0, -30, 250, Easing.CubicIn)
        );
        WeatherCard.IsVisible = false;

        // 3. Entrada del estado de carga e inicio de giro del spinner
        LoadingContainer.IsVisible = true;
        await LoadingContainer.FadeTo(1, 200);
        _ = IniciarRotacionSpinner();

        // 4. Ejecutar la simulación de carga (Task.Delay 2s en el ViewModel)
        await _viewModel.RefreshWeatherAsync();

        // 5. Salida del estado de carga y detención del spinner
        _isSpinning = false;
        await LoadingContainer.FadeTo(0, 200);
        LoadingContainer.IsVisible = false;

        // 6. Preparar posición inicial para la nueva tarjeta
        WeatherCard.TranslationY = 40;
        WeatherCard.IsVisible = true;

        // 7. Entrada de la nueva tarjeta con efecto Spring (rebote)
        await Task.WhenAll(
            WeatherCard.FadeTo(1, 450, Easing.CubicOut),
            WeatherCard.TranslateTo(0, 0, 450, Easing.SpringOut)
        );

        RefreshButton.IsEnabled = true;
    }

    private async Task IniciarRotacionSpinner()
    {
        _isSpinning = true;
        SpinnerIcon.Rotation = 0;

        while (_isSpinning)
        {
            // Rota 360 grados continuamente
            await SpinnerIcon.RotateTo(SpinnerIcon.Rotation + 360, 1000, Easing.Linear);
        }
    }
}