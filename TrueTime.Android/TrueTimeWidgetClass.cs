using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Widget;

namespace TrueTime.Droid
{
    [BroadcastReceiver(Label = "TrueTimeWidget")]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/true_time_widget_provider")]
    class TrueTimeWidget : AppWidgetProvider
    {
        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            context.StartService(new Intent(context, typeof(UpdateService)));

            //var me = new ComponentName(context, Java.Lang.Class.FromType(typeof(TrueTimeWidget)).Name);
            //appWidgetManager.UpdateAppWidget(me, UpdateTimes(context));
        }

        //public static RemoteViews UpdateTimes(Context context)
        public static void UpdateTimes(Context context)
        {
            var widgetView = new RemoteViews(context.PackageName, Resource.Layout.true_time_widget);

            MainPage.UpdateClock();

            widgetView.SetTextViewText(Resource.Id.TrueTimeText, MainPage.TrueTime.ToString("HH:mm:ss"));
            widgetView.SetTextViewText(Resource.Id.FalseTimeText, MainPage.FalseTime.ToString("HH:mm"));

            ComponentName thisWidget = new ComponentName(context, Java.Lang.Class.FromType(typeof(TrueTimeWidget)).Name);
            AppWidgetManager manager = AppWidgetManager.GetInstance(context);
            manager.UpdateAppWidget(thisWidget, widgetView);

            //return widgetView;
        }
    }
}