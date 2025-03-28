using SpeakAI.Services.Interfaces;
using SpeakAI.ViewModels;

namespace SpeakAI.Views;

public partial class WeatherPage : ContentPage
{
    public WeatherPage(WeatherViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
