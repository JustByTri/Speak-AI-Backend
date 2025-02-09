using System.Net.Http.Json;
using System.Text.Json;
using SpeakAI.Models;

namespace SpeakAI.Services;

public class WeatherService
{
    private readonly HttpClient _httpClient;

    public WeatherService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://192.168.14.133:7288/")
        };
    }

    public async Task<List<WeatherForecast>> GetWeatherAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("WeatherForecast");
            string json = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"🔍 API Response: {json}");

            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<List<WeatherForecast>>(json) ?? new List<WeatherForecast>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ API Error: {ex.Message}");
            return new List<WeatherForecast>();
        }
    }
}
