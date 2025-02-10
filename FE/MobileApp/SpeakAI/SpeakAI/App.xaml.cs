using SpeakAI.Services.Interfaces;

namespace SpeakAI
{
    public partial class App : Application
    {
        public App(IUserService userService, ILoginService loginService)
        {
            InitializeComponent();
            MainPage = new NavigationPage(new LoginPage(userService, loginService));
        }
    }
}
