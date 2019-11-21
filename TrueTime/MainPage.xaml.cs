using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace TrueTime
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        const int secondsInHalfDay = 43200;

        public MainPage()
        {
            InitializeComponent();

            Task.Run(async () => {
                while(true)
                {
                    string locationElemText;

                    try
                    {
                        var location = await Geolocation.GetLastKnownLocationAsync();

                        if (location != null)
                        {
                            locationElemText = $"lat: {location.Latitude}, long: {location.Longitude}";
                            UpdateClock(location.Longitude);
                        }
                        else
                        {
                            locationElemText = "Ei paikkatietoa";
                        }
                    }
                    catch (FeatureNotEnabledException fneEx)
                    {
                        locationElemText = fneEx.ToString();
                    }
                    catch (PermissionException pEx)
                    {
                        locationElemText = pEx.ToString();
                    }

                    // UI-eventit
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        LocationElem.Text = locationElemText;
                    });

                    // Ei tapeta prosessoria
                    await Task.Delay(1000);
                }
            });
        }

        private void UpdateClock(double longitude)
        {
            double modifier = longitude / 360d;
            double seconds = secondsInHalfDay * modifier;

            DateTime t = DateTime.UtcNow.AddSeconds(seconds);

            Device.BeginInvokeOnMainThread(() =>
            {
                ClockElem.Text = t.ToString("HH:mm:ss");
            });
        }
    }
}
