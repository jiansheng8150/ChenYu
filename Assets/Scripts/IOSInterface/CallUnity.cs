using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class CallUnity : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /**
     * IOS系统弹框回调
     * retStr 类型,按钮编号  如:unlogin,1
     * 类型：由Unity传过去的
     * 按钮编号：用户点击了第几个按钮， 编号从0开始
     * */
    public void OnSelectTitleDialogCallBack(string retStr)
    {
#if UNITY_IOS
        Debug.Log("OnSelectTitleDialogCallBack:" + retStr);
        char splitChar = ',';
        string[] srtArr = retStr.Split(splitChar);
        switch (srtArr[0])
        {
            case "unlogin":
                if (srtArr[1] == "1") // 退出登录
                {
                    NetWorkManager.Instance.UnLogin();
                }
                break;
            case "getNotificationPermission":
                //申请通知权限
                UnityEngine.iOS.NotificationServices.RegisterForNotifications(NotificationType.Alert | NotificationType.Badge | NotificationType.Sound);
                PlayerPrefs.SetString("isFirstStartCountDownd", "false");
                PlayerPrefs.Save();
                break;
            default:
                break;
        }
#endif
    }

    /**
     * IOS购买商品回调
     * retStr 返回结果,字符串，receipt为成功，2为失败，3为取消，4为重复购买，5为其他
     * */
    public void OnBuyProductCallBack(string retStr)
    {
#if UNITY_IOS
        Debug.Log("OnBuyProductCallBack:" + retStr);
        if (retStr.Length > 3) // 返回的是支付凭证，发送给服务器验证
        {
            if (CallIOS.curBuyProductId == Global.PRODUCTID1)
            {
                UserData.Instance.Gold += 240;
                JsonData order = new JsonData();
                order.SetJsonType(JsonType.Object);
                order["orderid"] = retStr;
                order["product_id"] = CallIOS.curBuyProductId;
                order["gold"] = 240;
                OrderManager.Instance.addOrder(order);
            }
            else if (CallIOS.curBuyProductId == Global.PRODUCTID2)
            {
                UserData.Instance.Gold += 720;
                JsonData order = new JsonData();
                order.SetJsonType(JsonType.Object);
                order["orderid"] = retStr;
                order["product_id"] = CallIOS.curBuyProductId;
                order["gold"] = 720;
                OrderManager.Instance.addOrder(order);
            }
            else if (CallIOS.curBuyProductId == Global.PRODUCTID3)
            {
                UserData.Instance.Gold += 1440;
                JsonData order = new JsonData();
                order.SetJsonType(JsonType.Object);
                order["orderid"] = retStr;
                order["product_id"] = CallIOS.curBuyProductId;
                order["gold"] = 1440;
                OrderManager.Instance.addOrder(order);
            }
            Debug.Log("购买成功！");
        }
        else if (retStr == "100")
        {
            CallIOS.ShowSelectTitleDialog("openIAP", "提示", "您的App内购买项目处以关闭状态，需要您在 设置->通用->访问限制->App内购买项目 中打开", "好的", null);
            Debug.Log("打开 设置->通用->访问限制->App内购买项目");
        }
        else
        {
            Debug.Log("购买失败！");
        }

        if (retStr != "5") //获取商品列表不清除商品id
        {
            CallIOS.curBuyProductId = "";
            UIManager.Instance.HidePanel("PanelWaitingText");
        }
#else
        Debug.Log("OnSelectTitleDialogCallBack:" + retStr);
        char splitChar = ',';
        string[] srtArr = retStr.Split(splitChar);
        switch (srtArr[0])
        {
            case "unlogin":
                if (srtArr[1] == "1") // 退出登录
                {
                    NetWorkManager.Instance.UnLogin();
                }
                break;
            default:
                break;
        }
#endif
    }

    /**
    * 测试IOS购买商品回调
    * retStr 返回结果,字符串，receipt为成功，2为失败，3为取消，4为重复购买，5为其他
    * */
    public static void OnBuyProductCallBack2(string retStr)
    {
#if UNITY_IOS
        Debug.Log("OnBuyProductCallBack2:" + retStr);
        if (retStr.Length > 3) // 返回的是支付凭证，发送给服务器验证
        {
            if (CallIOS.curBuyProductId == Global.PRODUCTID1)
            {
                UserData.Instance.Gold += 240;
                JsonData order = new JsonData();
                order.SetJsonType(JsonType.Object);
                order["orderid"] = retStr;
                order["product_id"] = CallIOS.curBuyProductId;
                order["gold"] = 240;
                OrderManager.Instance.addOrder(order);
            }
            else if (CallIOS.curBuyProductId == Global.PRODUCTID2)
            {
                UserData.Instance.Gold += 720;
                JsonData order = new JsonData();
                order.SetJsonType(JsonType.Object);
                order["orderid"] = retStr;
                order["product_id"] = CallIOS.curBuyProductId;
                order["gold"] = 720;
                OrderManager.Instance.addOrder(order);
            }
            else if (CallIOS.curBuyProductId == Global.PRODUCTID3)
            {
                UserData.Instance.Gold += 1440;
                JsonData order = new JsonData();
                order.SetJsonType(JsonType.Object);
                order["orderid"] = retStr;
                order["product_id"] = CallIOS.curBuyProductId;
                order["gold"] = 1440;
                OrderManager.Instance.addOrder(order);
            }
            Debug.Log("购买成功！");
        }
        else if (retStr == "100")
        {
            CallIOS.ShowSelectTitleDialog("openIAP", "提示", "您的App内购买项目处以关闭状态，需要您在 设置->通用->访问限制->App内购买项目 中打开", "好的", null);
            Debug.Log("打开 设置->通用->访问限制->App内购买项目");
        }
        else
        {
            Debug.Log("购买失败！");
        }

        if (retStr != "5") //获取商品列表不清除商品id
        {
            CallIOS.curBuyProductId = "";
            UIManager.Instance.HidePanel("PanelWaitingText");
        }
#endif
    }

    // 测试用
    public static void OnSelectTitleDialogCallBack2(string retStr)
    {
#if UNITY_IOS
        Debug.Log("OnSelectTitleDialogCallBack:" + retStr);
        char splitChar = ',';
        string[] srtArr = retStr.Split(splitChar);
        switch (srtArr[0])
        {
            case "unlogin":
                if (srtArr[1] == "1") // 退出登录
                {
                    NetWorkManager.Instance.UnLogin();
                }
                break;
            case "getNotificationPermission":
                //申请通知权限
                UnityEngine.iOS.NotificationServices.RegisterForNotifications(NotificationType.Alert | NotificationType.Badge | NotificationType.Sound);
                PlayerPrefs.SetString("isFirstStartCountDownd", "false");
                PlayerPrefs.Save();
                break;
            default:
                break;
        }
#else
        Debug.Log("OnSelectTitleDialogCallBack2:" + retStr);
        char splitChar = ',';
        string[] srtArr = retStr.Split(splitChar);
        switch (srtArr[0])
        {
            case "unlogin":
                if (srtArr[1] == "1") // 退出登录
                {
                    NetWorkManager.Instance.UnLogin();
                }
                break;
            default:
                break;
        }
#endif
    }

    // android调用unity
    public void GetToken(string token)
    {
        Debug.Log("Unity Debug GetToken:" + token);
        UIManager.Instance.ShowLog("GetToken:" + token);
    }
    public void ChangeToken(string token)
    {
        Debug.Log("Unity Debug ChangeToken:" + token);
        UIManager.Instance.ShowLog("ChangeToken:" + token);
    }
    public void OnTokenError(string msg)
    {
        Debug.Log("Unity Debug OnTokenError:" + msg);
        UIManager.Instance.ShowLog("OnTokenError:" + msg);
    }
}