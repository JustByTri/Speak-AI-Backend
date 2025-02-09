using SpeakAI.Services.Interfaces;

namespace SpeakAI;

public partial class LoginPage : ContentPage
{
    private readonly IUserService _userService;
	public LoginPage(IUserService userService)
	{
		InitializeComponent();
        _userService = userService;
	}
    private async void OnCreateAccountClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SignUpPage(_userService));
    }

    private async void OnSignInClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Notification", "Comming soon!", "Done" );
    }
}