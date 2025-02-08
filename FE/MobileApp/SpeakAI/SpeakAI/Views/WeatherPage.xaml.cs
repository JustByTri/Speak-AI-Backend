using SpeakAI.ViewModels;

namespace SpeakAI.Views;

public partial class WeatherPage : ContentPage
{
    public WeatherPage()
    {
        InitializeComponent();
        BindingContext = new WeatherViewModel();
    }
}
