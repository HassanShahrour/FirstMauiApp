using CommunityToolkit.Mvvm.ComponentModel;

namespace BMB.Utilities
{
    public partial class ThemeUtility : ObservableObject
    {
        private static ThemeUtility? _current;
        public static ThemeUtility Current => _current ??= new ThemeUtility();

        [ObservableProperty]
        private bool isDarkMode;

        partial void OnIsDarkModeChanged(bool value)
        {
            ThemeManager.SetTheme(value ? "Dark" : "Light");
            Preferences.Set("AppTheme", value ? "Dark" : "Light");
        }

        private ThemeUtility()
        {
            try
            {
                if (Preferences.ContainsKey("AppTheme"))
                {
                    string savedTheme = Preferences.Get("AppTheme", "Light");
                    IsDarkMode = savedTheme == "Dark";
                }
                else
                {
                    bool systemDark = Application.Current?.UserAppTheme == AppTheme.Dark;
                    IsDarkMode = systemDark;
                    Preferences.Set("AppTheme", systemDark ? "Dark" : "Light");
                }
            }
            catch
            {
                IsDarkMode = false;
            }
        }
    }
}
