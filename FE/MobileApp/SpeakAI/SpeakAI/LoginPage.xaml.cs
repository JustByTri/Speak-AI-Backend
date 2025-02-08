namespace SpeakAI;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}
    private async void OnCreateAccountClicked(object sender, EventArgs e)
    {
    }

    private async void OnSignInClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }
}