using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;
using UIKit;

using Firebase.Core;
using Firebase.CloudMessaging;
using UserNotifications;
using Xamarin.Essentials;
using AudioToolbox;
using App1.Views;
using App1.Clases;

namespace SolCom.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        public void DidRefreshRegistrationToken(Messaging messaging, string fcmToken)
        {
            Xamarin.Forms.Application.Current.Properties["Fcmtocken"] = fcmToken;
            Xamarin.Forms.Application.Current.SavePropertiesAsync();
            
            if ((App._JerarquiaUsuario != null) && (int)App._JerarquiaUsuario.iIdUsuario > 0)
            {
                WSClass wsClass = new WSClass();
                wsClass.GuardaToken((int)App._JerarquiaUsuario.iIdUsuario, (string)Xamarin.Forms.Application.Current.Properties["name"], (string)Xamarin.Forms.Application.Current.Properties["Fcmtocken"], 
                                    (string)Xamarin.Forms.Application.Current.Properties["device"], (string)Xamarin.Forms.Application.Current.Properties["platform"]);
            }
        }


        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {

            global::Xamarin.Forms.Forms.Init();
            Firebase.Core.App.Configure();

            LoadApplication(new App());

            // Register your app for remote notifications.
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // iOS 10 or later
                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) => {
                    Console.WriteLine(granted);
                });

                // For iOS 10 display notification (sent via APNS)
                UNUserNotificationCenter.Current.Delegate = this;

                // For iOS 10 data message (sent via FCM)
                Messaging.SharedInstance.Delegate = this;
            }
            else
            {
                // iOS 9 or before
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            UIApplication.SharedApplication.RegisterForRemoteNotifications();

            Firebase.InstanceID.InstanceId.Notifications.ObserveTokenRefresh((sender, e) =>
            {
                var newToken = Messaging.SharedInstance.FcmToken ?? "";
                Xamarin.Forms.Application.Current.Properties["Fcmtocken"] = newToken;
                Xamarin.Forms.Application.Current.SavePropertiesAsync();
            });

            var token = Messaging.SharedInstance.FcmToken ?? "";
            Xamarin.Forms.Application.Current.Properties["Fcmtocken"] = token;
            Xamarin.Forms.Application.Current.Properties["device"] = DeviceInfo.Model;
            Xamarin.Forms.Application.Current.Properties["platform"] = DeviceInfo.Platform.ToString();
            Xamarin.Forms.Application.Current.SavePropertiesAsync();
            Messaging.SharedInstance.Subscribe("news");

            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;

            return base.FinishedLaunching(app, options);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            Messaging.SharedInstance.ApnsToken = deviceToken;
            Analytics.TrackEvent("Token" + deviceToken.Description);

            var token = Messaging.SharedInstance.FcmToken ?? "";
            Xamarin.Forms.Application.Current.Properties["Fcmtocken"] = token;
            Xamarin.Forms.Application.Current.SavePropertiesAsync();

            NSUserDefaults.StandardUserDefaults.SetString(deviceToken.Description,"PushDeviceToken");
        }

        [Export("application:didReceiveRemoteNotification:fetchCompletionHandler:")]
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, System.Action<UIBackgroundFetchResult> completionHandler)
        {
            var result = Push.DidReceiveRemoteNotification(userInfo);
            if (result)
            {
                completionHandler?.Invoke(UIBackgroundFetchResult.NewData);
            }
            else
            {
                completionHandler?.Invoke(UIBackgroundFetchResult.NoData);
            }
        }

        //Handles a notification action selected by the user.  iOS 10+
        [Export("userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:")]
        public void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            //TAPPED NOTIFICATION

            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
            Analytics.TrackEvent("DidReceiveNotification");
            SystemSound.Vibrate.PlayAlertSound();
            SystemSound.Vibrate.PlaySystemSound();
            completionHandler();
            NSDictionary userInfo = response.Notification.Request.Content.UserInfo;
            var err = new NSError();
            var theJSONData = NSJsonSerialization.Serialize(userInfo, NSJsonWritingOptions.PrettyPrinted, out err);
            var theJSONText = new NSString(theJSONData, NSStringEncoding.UTF8);
            Analytics.TrackEvent("userinfo: " + theJSONText.ToString());
            RespuestaSolicitudes itemSolicitud = App.Current.CargaSolicitud(userInfo.ToDictionary(x => x.Key.ToString(), x => x.Value.ToString()));
            App.Current.MainPage = new MainPage(true, itemSolicitud);
            Analytics.TrackEvent("DidReceiveNotification2");
        }


        //Asks the delegate how to handle a notification that arrived while the app was running in the foreground.  iOS 10+
        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            SystemSound.Vibrate.PlayAlertSound();
            SystemSound.Vibrate.PlaySystemSound();
            completionHandler(UNNotificationPresentationOptions.Sound | UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Badge );

            NSDictionary userInfo = notification.Request.Content.UserInfo;
            var err = new NSError();
            var theJSONData = NSJsonSerialization.Serialize(userInfo, NSJsonWritingOptions.PrettyPrinted, out err);
            var theJSONText = new NSString(theJSONData, NSStringEncoding.UTF8);
            Analytics.TrackEvent("userinfo: " + theJSONText.ToString());
            RespuestaSolicitudes itemSolicitud = App.Current.CargaSolicitud(userInfo.ToDictionary(x => x.Key.ToString(), x => x.Value.ToString()));

        }

        [Export("messaging:didReceiveRegistrationToken:")]
        public void DidReceiveRegistrationToken(Messaging messaging, string fcmToken)
        {
            Xamarin.Forms.Application.Current.Properties["Fcmtocken"] = fcmToken;
            Xamarin.Forms.Application.Current.SavePropertiesAsync();

            if ((App._JerarquiaUsuario != null) && (int)App._JerarquiaUsuario.iIdUsuario > 0)
            {
                WSClass wsClass = new WSClass();
                wsClass.GuardaToken((int)App._JerarquiaUsuario.iIdUsuario, (string)Xamarin.Forms.Application.Current.Properties["name"], (string)Xamarin.Forms.Application.Current.Properties["Fcmtocken"], 
                                    (string)Xamarin.Forms.Application.Current.Properties["device"], (string)Xamarin.Forms.Application.Current.Properties["platform"]);
            }
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            Analytics.TrackEvent("ErrorRemote " + error.LocalizedDescription);
        }
    }
}
