using Android.App;
using Android.Content;
using AndroidX.Core.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMB.Platforms.Android
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter(new[] { "BMB.ALARM_TRIGGERED" })]
    public class AlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var title = intent.GetStringExtra("title") ?? "Reminder";
            var message = intent.GetStringExtra("message") ?? "You have an event.";

            var builder = new NotificationCompat.Builder(context, "default_channel")
                .SetContentTitle(title)
                .SetContentText(message)
                .SetSmallIcon(Resource.Drawable.notification_bg)
                .SetAutoCancel(true)
                .SetPriority((int)NotificationPriority.High);

            var notificationManager = NotificationManagerCompat.From(context);
            notificationManager.Notify(new Random().Next(), builder.Build());
        }
    }
}