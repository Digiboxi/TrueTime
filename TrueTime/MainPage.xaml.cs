using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Android;
using Android.Content.PM;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using Android.App;

namespace TrueTime
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        const int secondsInHalfDay = 43200;

        public static DateTime TrueTime { get; set; }
        public static DateTime FalseTime { get; set; }
        private static Location CurrentLocation { get; set; }

        public MainPage()
        {
            InitializeComponent();

            var requiredPermissions = new List<string>();

            if (ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.AccessCoarseLocation) != (int)Permission.Granted)
            {
                requiredPermissions.Add(Manifest.Permission.AccessCoarseLocation);
            }

            if (ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.AccessFineLocation) != (int)Permission.Granted)
            {
                requiredPermissions.Add(Manifest.Permission.AccessFineLocation);
            }

            if (!requiredPermissions.Any())
            {
                RunProgram();
            }
        }

        private void RunProgram()
        {
            Task.Run(async () => {
                while (true)
                {
                    string locationElemText;

                    try
                    {
                        UpdateClock();

                        locationElemText = $"lat: {CurrentLocation.Latitude}, long: {CurrentLocation.Longitude}";

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            ClockElem.Text = TrueTime.ToString("HH:mm:ss");
                        });
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
                        //LocationElem.Text = locationElemText;
                        // Kartan käyttö: https://www.youtube.com/watch?v=7qu1MHSsZ7Q
                    });

                    // Ei tapeta prosessoria
                    await Task.Delay(100);
                }
            });
        }

        public async static void UpdateClock()
        {
            CurrentLocation = await Geolocation.GetLastKnownLocationAsync();

            double modifier = CurrentLocation.Longitude / 180d; // Puolet täydestä ympyrästä, koska puolet aikavyöhykkeistä on miinusmerkkisillä asteluvuilla.
            double seconds = secondsInHalfDay * modifier; // Samoin lasketaan myös puolen vuorokauden sekuntien perusteella, koska modifier saattaa olla negatiivinen

            DateTime t = DateTime.UtcNow.AddSeconds(seconds);

            TrueTime = t;
            FalseTime = DateTime.Now;
        }
    }
}
