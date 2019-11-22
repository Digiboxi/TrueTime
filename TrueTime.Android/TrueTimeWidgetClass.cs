using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Widget;

namespace TrueTime.Droid
{
    [BroadcastReceiver(Label = "TrueTimeWidget")]
    [MetaData("android.appwidget.provider", Resource = "@xml/true_time_widget_provider")]
    class TrueTimeWidgetClass : AppWidgetProvider
    {
        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            var me = new ComponentName(context, Java.Lang.Class.FromType(typeof(TrueTimeWidgetClass)).Name);
            appWidgetManager.UpdateAppWidget(me, UpdateTimes(context));
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