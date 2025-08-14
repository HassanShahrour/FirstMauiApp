using BMB.DatabaseHelper.Interfaces;
using BMB.Utilities;

namespace BMB
{

    public partial class App : Application
    {
        private readonly IAppDatabase _db;
        private readonly UserUtility _userUtility;

        public App(IAppDatabase db, UserUtility userUtility)
        {
            InitializeComponent();

            _db = db;
            _userUtility = userUtility;

            MainPage = new Views.SplashPage();

            InitializeAppAsync();
        }


        private async void InitializeAppAsync()
        {
            await _db.InitAsync();

            var isLoggedIn = await IsloggedIn();
            if (isLoggedIn)
            {
                SwitchToAppShell();
            }
            else
            {
                SwitchToAuthShell();
            }
        }

        private async Task<bool> IsloggedIn()
        {
            return await _userUtility.GetLoggedInUserIdAsync() > 0;
        }

        public static void SwitchToAppShell()
        {
            Current.MainPage = new AppShell();
        }

        public static void SwitchToAuthShell()
        {
            Current.MainPage = new AuthShell();
        }
    }


}
