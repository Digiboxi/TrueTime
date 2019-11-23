using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Widget;
using System.Threading.Tasks;

namespace TrueTime.Droid
{
    [BroadcastReceiver(Label = "TrueTimeWidget")]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/true_time_widget_provider")]
    class TrueTimeWidgetClass : AppWidgetProvider
    {
        private bool running = false;

        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            StartUpdateTimes(context, appWidgetManager);
        }

        private void StartUpdateTimes(Context context, AppWidgetManager appWidgetManager)
        {
            if (!running)
            {
                running = true;

                Task.Run(async () =>
                {
                    while (true)
                    {
                        var me = new ComponentName(context, Java.Lang.Class.FromType(typeof(TrueTimeWidgetClass)).Name);
                        appWidgetManager.UpdateAppWidget(me, UpdateTimes(context));

                        // Ei tapeta prosessoria
                        await Task.Delay(500);
                    }
                });
            }
        }

        private RemoteViews UpdateTimes(Context context)
        {
            var widgetView = new RemoteViews(context.PackageName, Resource.Layout.true_time_widget);

            MainPage.UpdateClock();

            widgetView.SetTextViewText(Resource.Id.TrueTimeText, MainPage.TrueTime.ToString("HH:mm:ss"));
            widgetView.SetTextViewText(Resource.Id.FalseTimeText, MainPage.FalseTime.ToString("HH:mm"));

            return widgetView;
        }
    }
}