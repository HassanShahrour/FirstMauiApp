using BMB.DatabaseHelper.Interfaces;
using BMB.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;

namespace BMB.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        public ICommand LoginCommand { get; }

        private readonly IUserRepository _repository;
        private readonly UserUtility _userUtility;

        public LoginViewModel(IUserRepository repository, UserUtility userUtility)
        {
            _repository = repository;
            _userUtility = userUtility;
            LoginCommand = new Command(async () => await LoginUserAsync());
        }

        [RelayCommand]
        private async Task LoginUserAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {

                var user = await _repository.GetUserByEmail(Email);
                if (user == null)
                {
                    await Shell.Current.DisplayAlert("Error", "Login failed. Please try again.", "OK");
                    return;
                }

                var hashedInputPassword = HashPassword(Password);

                if (user.Password == hashedInputPassword)
                {
                    await _userUtility.SetLoggedInUserIdAsync(user.Id);
                    App.SwitchToAppShell();
                    await Shell.Current.GoToAsync("//Orders");
                    await Shell.Current.DisplayAlert("Success", "Login Successfull!", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Invalid Credentials.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Login error: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Login failed. Please try again.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task NavigateToRegisterAsync()
        {
            await Shell.Current.GoToAsync($"//Register");
        }

        [RelayCommand]
        public async Task LoginWithBiometricsAsync()
        {
            if (IsBusy) return;

            try
            {
                var isAvailable = await CrossFingerprint.Current.IsAvailableAsync(true);
                if (!isAvailable)
                {
                    await Shell.Current.DisplayAlert("Error", "Biometric authentication is not available on this device.", "OK");
                    return;
                }

                var authRequestConfig = new AuthenticationRequestConfiguration(
                    "Authentication Required",
                    "Use biometrics to login")
                {
                    CancelTitle = "Cancel",
                    FallbackTitle = "Use Passcode"
                };

                var result = await CrossFingerprint.Current.AuthenticateAsync(authRequestConfig);

                if (result.Authenticated)
                {
                    IsBusy = true;
                    await _userUtility.SetLoggedInUserIdAsync(1);
                    App.SwitchToAppShell();
                    await Shell.Current.GoToAsync("//Orders");
                    await Shell.Current.DisplayAlert("Success", "Login Successfull!", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Failed", "Authentication failed.", "Try again");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Authentication error: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }

        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
