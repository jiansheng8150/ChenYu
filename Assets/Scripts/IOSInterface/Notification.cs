using System;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class Notification:MonoBehaviour
{
    void Awake()
    {
#if UNITY_IOS
        //非首次登录申请通知权限（容错）
        if (Global.IsFirstStartCountDownd() == false)
        {

            UnityEngine.iOS.NotificationServices.RegisterForNotifications(NotificationType.Alert | NotificationType.Badge | NotificationType.Sound);

    }

    //第一次进入游戏的时候清空，有可能用户自己把游戏冲后台杀死，这里强制清空
    CleanNotification();
#endif
    }

    //本地推送(指定时间通知)
    public static void NotificationMessage(string message, int hour, int minute, int secend, string soundName, bool isRepeatDay, bool showIconBadgeNumber = true)
    {
        #if UNITY_IOS
        int year = DateTime.Now.Year;
        int month = DateTime.Now.Month;
        int day = DateTime.Now.Day;
        DateTime newDate = new DateTime(year, month, day, hour, minute, secend);
        NotificationMessage(message, newDate, soundName, isRepeatDay, showIconBadgeNumber);
        #endif
    }

    //本地推送 你可以传入一个固定的推送时间
    public static void NotificationMessage(string message, DateTime newDate, string soundName, bool isRepeatDay, bool showIconBadgeNumber = true)
    {
#if UNITY_IOS
        if (Global.IsOpenNotification() == false)
        {
            return;
        }
        //推送时间需要大于当前时间
        if (newDate > DateTime.Now)
        {

            UnityEngine.iOS.LocalNotification localNotification = new UnityEngine.iOS.LocalNotification();

            localNotification.fireDate = newDate;
            localNotification.alertBody = message;
            if (showIconBadgeNumber)
            {
                localNotification.applicationIconBadgeNumber = 1;
            }
            else
            {
                localNotification.applicationIconBadgeNumber = -1;
            }
            localNotification.hasAction = true;
            localNotification.alertAction = "Tomato标题";
            if (isRepeatDay)
            {
                //是否每天定期循环
                localNotification.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.ChineseCalendar;
                localNotification.repeatInterval = UnityEngine.iOS.CalendarUnit.Day;
            }
            localNotification.soundName = soundName;
            UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(localNotification);
        }
#endif
    }

    //清空所有本地消息
    public static void CleanNotification()
    {
#if UNITY_IOS
        UnityEngine.iOS.LocalNotification localNotification = new UnityEngine.iOS.LocalNotification();
        localNotification.applicationIconBadgeNumber = -1;
        UnityEngine.iOS.NotificationServices.PresentLocalNotificationNow(localNotification);
        UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();
        UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
#endif
    }
}