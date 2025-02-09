using SpeakAI.ViewModels;

namespace SpeakAI;

public partial class SignUpPage : ContentPage
{
	public SignUpPage()
	{
		InitializeComponent();
        BindingContext = new SignUpViewModel();
    }
}