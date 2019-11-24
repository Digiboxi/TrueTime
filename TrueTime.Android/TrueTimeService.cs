using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using System;

namespace TrueTime.Droid
{
    [Service]
    public class UpdateService : Service
    {
        [Obsolete]
        public override void OnStart(Intent intent, int startId)
        {
            Intent alarmIntent = new Intent(this, typeof(RepeatingAlarm));
            var source = PendingIntent.GetBroadcast(this, 0, alarmIntent, 0);
            AlarmManager alarmManager = GetSystemService(AlarmService).JavaCast<AlarmManager>();

            //           SetRepeating([GeneratedEnum] AlarmType type, long triggerAtMillis, long intervalMillis, PendingIntent operation);
            alarmManager.SetRepeating(AlarmType.Rtc, SystemClock.CurrentThreadTimeMillis(), 500, source);
            //alarmManager.SetExact(AlarmType.Rtc, SystemClock.CurrentThreadTimeMillis(), source);
        }

        public override IBinder OnBind(Intent intent)
        {
            // We don't need to bind to this service
            return null;
        }
    }


    [BroadcastReceiver]
    public class RepeatingAlarm : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            TrueTimeWidget.UpdateTimes(context);
        }
    }
}