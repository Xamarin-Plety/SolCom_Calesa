using Foundation;
using SolCom.Clases;
using SolCom.iOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

[assembly: Dependency(typeof(PushCancelService))]
namespace SolCom.iOS
{
    public class PushCancelService : IPushCancel
    {
        private const string NotificationKey = "LocalNotificationKey";
        public void CancelPush(int id, int iCount)
        {
            UNUserNotificationCenter.Current.RemoveAllPendingNotificationRequests();
            UNUserNotificationCenter.Current.RemoveAllDeliveredNotifications();
            UIApplication.SharedApplication.CancelAllLocalNotifications();
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;


            //var notifications = UIApplication.SharedApplication.ScheduledLocalNotifications;
            //var notification = notifications.Where(n => n.UserInfo.ContainsKey(NSObject.FromObject(NotificationKey))).FirstOrDefault(n => n.UserInfo[NotificationKey].Equals(NSObject.FromObject(id)));
            //UIApplication.SharedApplication.CancelAllLocalNotifications();
            //if (notification != null)
            //{
            //    UIApplication.SharedApplication.CancelLocalNotification(notification);
            //    UIApplication.SharedApplication.CancelAllLocalNotifications();
            //}
        }
    }
}