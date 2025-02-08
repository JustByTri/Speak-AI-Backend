using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SpeakAI.Services;
using SpeakAI.ViewModels;

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
            builder.Services.AddSingleton<WeatherService>();
            builder.Services.AddSingleton<WeatherViewModel>();
            return builder.Build();
        }
    }
}
