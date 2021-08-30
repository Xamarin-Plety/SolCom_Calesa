using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Util;
using Firebase.Iid;
using App1.Clases;

namespace SolCom.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    class FirebaseService : FirebaseInstanceIdService
    {

        const string TAG = "MyFirebaseIIDService";

        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Log.Debug(TAG, "Refreshed token: " + refreshedToken);
            SendRegistrationToServer(refreshedToken);
        }

        void SendRegistrationToServer(string token)
        {
            // Add custom implementation, as needed.
            Xamarin.Forms.Application.Current.Properties["Fcmtocken"] = token;
            Xamarin.Forms.Application.Current.SavePropertiesAsync();

            if ((App._JerarquiaUsuario != null) && (int)App._JerarquiaUsuario.iIdUsuario > 0)
            {
                WSClass wsClass = new WSClass();
                wsClass.GuardaToken((int)App._JerarquiaUsuario.iIdUsuario, (string)Xamarin.Forms.Application.Current.Properties["name"], (string)Xamarin.Forms.Application.Current.Properties["Fcmtocken"], 
                        (string)Xamarin.Forms.Application.Current.Properties["device"], (string)Xamarin.Forms.Application.Current.Properties["platform"]);
            }

        }
    }
}