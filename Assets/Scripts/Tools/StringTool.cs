using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class StringTool
{
    private static int seed = 1;
    public static string FormatTime(int seconds)
    {
        string ret = "";
        int minute = (int)seconds / 60;
        int second = (int)seconds % 60;
        if (minute < 10)
        {
            ret += "0" + minute;
        }
        else
        {
            ret += "" + minute;
        }
        ret += ":";
        if (second < 10)
        {
            ret += "0" + second;
        }
        else
        {
            ret += "" + second;
        }
        return ret;
    }

    public static string FormatTime2(int seconds)
    {
        string ret = "";
        int hour = (int)seconds / 3600;
        int minute = (int)((seconds % 3600) / 60);
        if (hour > 0)
        {
            ret += hour + "小时";
        }
        if (minute > 0)
        {
            ret += minute + "分钟";
        }
        return ret;
    }

    public static string getUid()
    {
        DateTime dt0 = new DateTime(1970, 1, 1,8,0,0);
        TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - dt0.Ticks);
        int dateVal = (int)ts.TotalSeconds;
        Random ramdom = new Random(dateVal+seed);
        string uid = "uid_"+ dateVal+ "_"+ seed + "_"+ ramdom.Next(1, 100000);
        seed++;
        return uid;
    }
}
