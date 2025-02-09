using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SpeakAI.Services;
using SpeakAI.Services.Interfaces;
using SpeakAI.Services.Service;
using SpeakAI.ViewModels;
using SpeakAI.Views;

namespace SpeakAI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            
#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<HttpClient>(sp =>
            {
                return new HttpClient { BaseAddress = new Uri("http://192.168.15.131:7288/") };
            });
            builder.Services.AddSingleton<IWeatherService, WeatherService>();
            builder.Services.AddSingleton<WeatherViewModel>();
            builder.Services.AddTransient<WeatherPage>();
            return builder.Build();
        }
    }
}
