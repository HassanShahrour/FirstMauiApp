using BMB.Resources;
using System.ComponentModel;
using System.Globalization;

namespace BMB.Utilities
{
    public class LocalizationUtility : INotifyPropertyChanged
    {
        public static LocalizationUtility Instance { get; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public string this[string key] =>
            Strings.ResourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? $"[{key}]";

        public void ChangeCulture(string languageCode)
        {
            var newCulture = new CultureInfo(languageCode);
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }
}
