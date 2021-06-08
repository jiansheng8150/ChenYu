using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour {

    //金币
    public Text TextGold;
    private int isPlayGoldChange = 0;
    private int curShowGold;
    private int willShowGold;

    void Awake()
    {
        DataChange.Addlistener("gold", OnGoldChange);
    }

    // Use this for initialization
    void Start () {
        TextGold.text = UserData.Instance.Gold.ToString();
        curShowGold = UserData.Instance.Gold;
    }

    // Update is called once per frame
    void Update () {
        if (isPlayGoldChange == 1)
        {
            willShowGold = Mathf.CeilToInt(Mathf.Lerp(curShowGold, UserData.Instance.Gold, 0.1f));
            if (willShowGold == curShowGold || curShowGold >= UserData.Instance.Gold || Mathf.Abs(curShowGold - UserData.Instance.Gold) <= 1)
            {
                curShowGold = UserData.Instance.Gold;
                TextGold.text = curShowGold.ToString();
                isPlayGoldChange = 0;
                return;
            }
            curShowGold = willShowGold;
            TextGold.text = curShowGold.ToString();
        }
        else if(isPlayGoldChange == -1)
        {
            willShowGold = Mathf.CeilToInt(Mathf.Lerp(curShowGold, UserData.Instance.Gold, 0.1f));
            if (willShowGold == curShowGold || curShowGold <= UserData.Instance.Gold || Mathf.Abs(curShowGold - UserData.Instance.Gold) <= 1)
            {
                curShowGold = UserData.Instance.Gold;
                TextGold.text = curShowGold.ToString();
                isPlayGoldChange = 0;
                return;
            }
            curShowGold = willShowGold;
            TextGold.text = curShowGold.ToString();
        }
    }

    private void OnGoldChange(object value)
    {
        if (curShowGold <= UserData.Instance.Gold)
        {
            isPlayGoldChange = 1;
        }
        else
        {
            isPlayGoldChange = -1;
        }
    }

    //点击登录
    public void ClickButtonLogin()
    {
        AudioManager.Instance.PlayEffect("click");
        UIManager.Instance.ShowPanel("CanvasUser", false);
        UIManager.Instance.ShowPanel("CanvasLoginMain", false);
    }

    //点击开始
    public void ClickButtonStart()
    {
        AudioManager.Instance.PlayEffect("click");
        UIManager.Instance.ShowPanel("PanelGame", true);

        //AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        //AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        //if (jo != null)
        //{
        //    jo.Call("unityCall");
        //}
    }

    //点击商城
    public void ClickButtonShop()
    {
#if UNITY_ANDROID

#else
        AudioManager.Instance.PlayEffect("click");
        UIManager.Instance.ShowPanel("PanelShop", false);
#endif
    }

    //点击设置
    public void ClickButtonSet()
    {
        AudioManager.Instance.PlayEffect("click");
        UIManager.Instance.ShowPanel("PanelSetting", false);
    }
}
