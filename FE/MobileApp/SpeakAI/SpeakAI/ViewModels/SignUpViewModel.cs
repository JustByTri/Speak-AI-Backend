using SpeakAI.Services.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SpeakAI.ViewModels
{
    public class SignUpViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _email;
        private string _fullName;
        private DateTime _birthday = DateTime.Today;
        private string _gender;
        private string _password;
        private string _confirmPassword;
        private string _notificationMessage;
        public event PropertyChangedEventHandler PropertyChanged;
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }

        public string FullName
        {
            get => _fullName;
            set { _fullName = value; OnPropertyChanged(nameof(FullName)); }
        }

        public DateTime Birthday
        {
            get => _birthday;
            set { _birthday = value; OnPropertyChanged(nameof(Birthday)); }
        }

        public string Gender
        {
            get => _gender;
            set { _gender = value; OnPropertyChanged(nameof(Gender)); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set { _confirmPassword = value; OnPropertyChanged(nameof(ConfirmPassword)); }
        }
        public string NotificationMessage
        {
            get => _notificationMessage;
            set { _notificationMessage = value; OnPropertyChanged(nameof(NotificationMessage)); }
        }
        public ICommand SignUpCommand { get; }
        public ICommand SignInCommand { get; }
        public SignUpViewModel()
        {
            SignUpCommand = new Command(OnSignUp);
            SignInCommand = new Command(OnSignIn);
        }
        private async void OnSignIn()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        private async void OnSignUp()
        {
            if (string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(FullName) ||
                string.IsNullOrWhiteSpace(Gender) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                NotificationMessage = "All fields are required.";
                return;
            }

            if (Password != ConfirmPassword)
            {
                NotificationMessage = "Passwords do not match.";
                return;
            }

            var newUser = new UserDTO
            {
                username = Username,
                password = Password,
                confirmedPassword = ConfirmPassword,
                email = Email,
                fullName = FullName,
                birthday = Birthday.ToString(),
                gender = Gender
            };

            NotificationMessage = "Account created successfully!";
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
