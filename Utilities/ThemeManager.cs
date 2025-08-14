using BMB.Resources.Themes;

namespace BMB.Utilities
{
    public static class ThemeManager
    {
        public static void SetTheme(string theme)
        {
            ResourceDictionary themeDict = theme switch
            {
                "Dark" => new DarkTheme(),
                _ => new LightTheme()
            };

            Shell.Current.Resources.MergedDictionaries.Clear();
            Shell.Current.Resources.MergedDictionaries.Add(themeDict);

        }
    }
}
