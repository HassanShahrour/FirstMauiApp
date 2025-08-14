using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
using Plugin.Fingerprint;
namespace BMB
{
    [Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    Icon = "@mipmap/appicon",
    ConfigurationChanges = ConfigChanges.ScreenSize
        | ConfigChanges.Orientation
        | ConfigChanges.UiMode
        | ConfigChanges.ScreenLayout
        | ConfigChanges.SmallestScreenSize
        | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            CrossFingerprint.SetCurrentActivityResolver(() => this);
            LocalNotificationCenter.NotifyNotificationTapped(Intent);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel("default_channel", "General", NotificationImportance.High)
                {
                    Description = "General Notifications"
                };
                var manager = (NotificationManager)GetSystemService(NotificationService);
                manager.CreateNotificationChannel(channel);
            }

        }
    }

}
