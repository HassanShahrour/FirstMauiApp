using Android.App;
using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMB.Platforms.Android
{
    public class AndroidAlarmService
    {
        public void ScheduleAlarm(string title, string description, DateTime notifyTime)
        {
            var context = Platform.AppContext;

            Intent intent = new(context, typeof(AlarmReceiver));
            intent.SetAction("BMB.ALARM_TRIGGERED");
            intent.PutExtra("title", title);
            intent.PutExtra("message", description);

            int requestCode = int.Parse(DateTime.Now.ToString("MMddHHmmss"));
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(context, requestCode, intent, PendingIntentFlags.Immutable | PendingIntentFlags.UpdateCurrent);

            long triggerTime = new DateTimeOffset(notifyTime).ToUnixTimeMilliseconds();

            AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);
            alarmManager.SetExact(AlarmType.RtcWakeup, triggerTime, pendingIntent);
        }
    }
}
