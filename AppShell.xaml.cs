using BMB.Utilities;
using BMB.Views;

namespace BMB
{
    public partial class AppShell : Shell
    {
        private readonly UserUtility _userUtility = new();

        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext == null)
            {
                BindingContext = ThemeUtility.Current;
            }
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(ProductSelectionPage), typeof(ProductSelectionPage));
        }

        protected override async void OnNavigating(ShellNavigatingEventArgs args)
        {
            if (args.Target.Location.OriginalString.Contains("Logout", StringComparison.OrdinalIgnoreCase))
            {
                args.Cancel();
                await HandleLogoutAsync();
            }

            base.OnNavigating(args);
        }

        private async Task HandleLogoutAsync()
        {
            _userUtility.ClearLoggedInUser();
            App.SwitchToAuthShell();
            await Shell.Current.GoToAsync("//Login");
        }
    }
}
