using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SpeakAI.Models;
using SpeakAI.Services;

namespace SpeakAI.ViewModels;

public class WeatherViewModel : INotifyPropertyChanged
{
    private readonly WeatherService _weatherService;
    public ObservableCollection<WeatherForecast> WeatherData { get; } = new();
    public ICommand LoadWeatherCommand { get; }

    public WeatherViewModel()
    {
        _weatherService = new WeatherService();
        LoadWeatherCommand = new Command(async () => await LoadWeather());
        _ = LoadWeather(); // Auto-load data on init
    }

    private async Task LoadWeather()
    {
        var weatherList = await _weatherService.GetWeatherAsync();
        WeatherData.Clear();
        foreach (var item in weatherList)
        {
            WeatherData.Add(item);
        }
        OnPropertyChanged(nameof(WeatherData));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
