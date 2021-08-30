using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using SolCom.Clases;
using SolCom.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;

[assembly: Dependency(typeof(PushCancelService))]
namespace SolCom.Droid
{
    public class PushCancelService: IPushCancel
    {
        const string channelId = "default";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";

        public const string TitleKey = "title";
        public const string MessageKey = "message";

        bool channelInitialized = false;
        int messageId = 0;
        int pendingIntentId = 0;

        NotificationManager manager;


        public void CancelPush(int id, int iCount)
        {
            try
            {
                //if(!channelInitialized)
                //CreateNotificationChannel();

                var notificationManager = NotificationManagerCompat.From(Android.App.Application.Context);
                notificationManager.CancelAll();

                //Show("Solicitudes Pendientes", iCount.ToString(), iCount);

            }
            catch (Exception ex)
            {
                string sError = ex.Message;
            }


        }


        public void Show(string title, string message, int iCount) 
        {
            Intent intent = new Intent(Android.App.Application.Context, typeof(MainActivity));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);

            PendingIntent pendingIntent = PendingIntent.GetActivity(Android.App.Application.Context, pendingIntentId++, intent, PendingIntentFlags.UpdateCurrent);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(Android.App.Application.Context, channelId)
                //.SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetTicker(title)
                .SetNumber(iCount)
                .SetSmallIcon(Resource.Drawable.icon);

            Notification notification = builder.Build();
            manager.Notify(messageId++, notification);
        }

        void CreateNotificationChannel()
        {
            manager = (NotificationManager)Android.App.Application.Context.GetSystemService(Android.App.Application.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(channelName);
                var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Low)
                {
                    Description = channelDescription
                };
                manager.CreateNotificationChannel(channel);
            }

            channelInitialized = true;
        }
    }
}