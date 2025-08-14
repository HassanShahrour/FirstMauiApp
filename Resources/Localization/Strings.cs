using System.Globalization;
using System.Resources;

namespace BMB.Resources.Localization
{
    public static class Strings
    {
        private static readonly ResourceManager resourceManager =
            new ResourceManager("BMB.Resources.Strings", typeof(Strings).Assembly);

        public static string Get(string key)
        {
            return resourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? key;
        }
    }
}
