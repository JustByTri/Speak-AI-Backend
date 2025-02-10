using SpeakAI.Services.Interfaces;
using SpeakAI.ViewModels;

namespace SpeakAI;

public partial class LoginPage : ContentPage
{
    private readonly IUserService _userService;
    private readonly ILoginService _loginService;
	public LoginPage(IUserService userService, ILoginService loginService)
	{
		InitializeComponent();
        _userService = userService;
        _loginService = loginService;
        BindingContext = new SignInViewModel(loginService, userService);
    }
    private async void OnCreateAccountClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SignUpPage(_userService));
    }
}