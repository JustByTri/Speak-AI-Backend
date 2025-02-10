using SpeakAI.Services.Interfaces;
using SpeakAI.ViewModels;

namespace SpeakAI;

public partial class LoginPage : ContentPage
{
    private readonly IUserService _userService;
	public LoginPage(IUserService userService, ILoginService loginService)
	{
		InitializeComponent();
        _userService = userService;
        BindingContext = new SignInViewModel(loginService);
    }
    private async void OnCreateAccountClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SignUpPage(_userService));
    }
}