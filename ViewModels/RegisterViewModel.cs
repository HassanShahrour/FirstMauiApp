using BMB.Data.Models;
using BMB.DatabaseHelper.Interfaces;
using BMB.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;

namespace BMB.ViewModels
{
    public partial class RegisterViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string fullName;

        [ObservableProperty]
        private string phoneNumber;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        public ICommand RegisterCommand { get; }

        private readonly IUserRepository _repository;
        private readonly UserUtility _userUtility;

        public RegisterViewModel(IUserRepository repository, UserUtility userUtility)
        {
            _repository = repository;
            _userUtility = userUtility;
            RegisterCommand = new Command(async () => await RegisterUserAsync());
        }

        [RelayCommand]
        private async Task RegisterUserAsync()
        {
            try
            {
                var hashedPassword = HashPassword(Password);

                var user = new User
                {
                    FullName = FullName,
                    PhoneNumber = PhoneNumber,
                    Email = Email,
                    Password = hashedPassword,
                    CreatedAt = DateTime.Now
                };

                await _repository.AddUser(user);
                await _userUtility.SetLoggedInUserIdAsync(user.Id);
                App.SwitchToAppShell();
                await Shell.Current.GoToAsync("//Orders");
                await Shell.Current.DisplayAlert("Success", "User registered!", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Registration error: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Registration failed. Please try again.", "OK");
            }
        }

        [RelayCommand]
        private async Task NavigateToLoginAsync()
        {
            await Shell.Current.GoToAsync("//Login");
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
