namespace SpeakAI;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}
    private async void OnCreateAccountClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SignUpPage());
    }

    private async void OnSignInClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Notification", "Comming soon!", "Done" );
    }
}