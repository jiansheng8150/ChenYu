using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Global
{
    //用户id(即电子邮件)
    public static string playerid;

    //用户密码
    public static string password;

    //订单信息
    public static string PRODUCTID1 = "com.jianshenglin.idiom.gold.1";
    public static string PRODUCTID2 = "com.jianshenglin.idiom.gold.2";
    public static string PRODUCTID3 = "com.jianshenglin.idiom.gold.3";

    //当前选择的关卡
    public static int curgk = -1;

    //用户id
    public static string Playerid()
    {
        return playerid;
    }

    //用户密码
    public static string Password()
    {
        return password;
    }
    
    //用户是否登录
    public static bool IsLogin()
    {
        if (PlayerPrefs.HasKey("isLogin"))
        {
            return PlayerPrefs.GetString("isLogin") == "true";
        }
        return false;
    }

    //用户是否第一次点击开始按钮
    public static bool IsFirstStartCountDownd()
    {
        if (PlayerPrefs.HasKey("isFirstStartCountDownd") == false)
        {
            return true;
        }
        return PlayerPrefs.GetString("isFirstStartCountDownd") == "true";
    }

    //用户是否开启音乐
    public static bool IsPlayMusic()
    {
        if (PlayerPrefs.HasKey("IsPlayMusic") == false)
        {
            return true;
        }
        return PlayerPrefs.GetString("IsPlayMusic") == "true";
    }

    //用户是否开启音效
    public static bool IsPlaySoundEffect()
    {
        if (PlayerPrefs.HasKey("IsPlaySoundEffect") == false)
        {
            return true;
        }
        return PlayerPrefs.GetString("IsPlaySoundEffect") == "true";
    }

    //用户是否开启通知
    public static bool IsOpenNotification()
    {
        if (PlayerPrefs.HasKey("IsOpenNotification") == false)
        {
            return true;
        }
        return PlayerPrefs.GetString("IsOpenNotification") == "true";
    }

    //存储今天专注的时间
    public static void setFocuseTime(int seconds)
    {
        int year = DateTime.Now.Year;
        int month = DateTime.Now.Month;
        int day = DateTime.Now.Day;
        string key = "focuseTime:" + year + "-" + month + "-" + day;
        if (PlayerPrefs.HasKey(key) == false)
        {
            PlayerPrefs.SetInt(key, seconds);
        }
        else
        {
            int total = PlayerPrefs.GetInt(key);
            total += seconds;
            PlayerPrefs.SetInt(key, total);
        }
        PlayerPrefs.Save();
    }

    //获取今天专注的时间
    public static int getFocuseTime()
    {
        int year = DateTime.Now.Year;
        int month = DateTime.Now.Month;
        int day = DateTime.Now.Day;
        string key = "focuseTime:" + year + "-" + month + "-" + day;
        int total = 0;
        if (PlayerPrefs.HasKey(key) == true)
        {
            total = PlayerPrefs.GetInt(key);
        }
        return total;
    }
}
