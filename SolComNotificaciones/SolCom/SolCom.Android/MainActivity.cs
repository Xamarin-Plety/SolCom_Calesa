using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Common;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;
using Microsoft.AppCenter.Push;
using Acr.UserDialogs;
using Xamarin.Essentials;
using SolCom.Clases;

namespace SolCom.Droid
{
    [Activity(Label = "Autoriza Compras", Icon = "@mipmap/ic_launcher", Theme = "@style/Theme.Splash", LaunchMode = LaunchMode.SingleTop, MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            string sMainAndroid = " Main Android Inicia : " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
            base.Window.RequestFeature(WindowFeatures.ActionBar);
            // Name of the MainActivity theme you had there before.
            base.SetTheme(Resource.Style.MainTheme);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            UserDialogs.Init(this);
            LoadApplication(new App());
            CheckForGoogleServices();
            const string TAG = "MyFirebaseIIDService";
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Android.Util.Log.Debug(TAG, "Refreshed token: " + refreshedToken);
            Xamarin.Forms.Application.Current.Properties["Fcmtocken"] = refreshedToken;
            Xamarin.Forms.Application.Current.Properties["device"] = DeviceInfo.Model;
            Xamarin.Forms.Application.Current.Properties["platform"] = DeviceInfo.Platform.ToString();
            Xamarin.Forms.Application.Current.SavePropertiesAsync();
            FirebaseMessaging.Instance.SubscribeToTopic("news");
            sMainAndroid += " Main Android Termina : " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
            cLog oLog = new cLog(sMainAndroid);
        }

        // Check if the Google Play Services is available to recieve Push Notification from Firebase
        public bool CheckForGoogleServices()
        {
            
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    Toast.MakeText(this, GoogleApiAvailability.Instance.GetErrorString(resultCode), ToastLength.Long);
                }
                else
                {
                    Toast.MakeText(this, "This device does not support Google Play Services", ToastLength.Long);
                }
                return false;
            }
            

            return true;
        }


        protected override void OnNewIntent(Android.Content.Intent intent)
        {
            base.OnNewIntent(intent);
            Push.CheckLaunchedFromNotification(this, intent);
        }

    }
}
