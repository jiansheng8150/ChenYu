using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OrderManager:MonoBehaviour
{
    private static OrderManager instance;

    public static OrderManager Instance
    {
        get
        {
            return instance;
        }

        set
        {
            instance = value;
        }
    }

    //推送订单间隔
    private int pushLoginOrderTime = 10*60;
    private int curPushLoginOrderPassTime = 0;

    void Awake()
    {
        instance = this;
    }
    
    void Update()
    {
        curPushLoginOrderPassTime++;
        if (curPushLoginOrderPassTime >= pushLoginOrderTime)
        {
            curPushLoginOrderPassTime = 0;
            PushLoginOrderNow();
        }
    }

    private void PushLoginOrderNow()
    {
        if (Global.IsLogin() && Global.Playerid() != null && Global.Password() != null)
        {
            JsonData orders = getOrdersByKey("loginOrders");
            if (orders.Count > 0)
            {
                for (int i = 0; i < orders.Count; i++)
                {
                    string orderStr = orders[i].ToJson();
                    StartCoroutine(PushLoginOrder(Global.Playerid(), Global.Password(), orderStr));
                }
            }
            JsonData createOrders = getOrdersByKey("unLoginOrders");
            if (createOrders.Count > 0)
            {
                for (int i = 0; i < createOrders.Count; i++)
                {
                    string orderStr = createOrders[i].ToJson();
                    StartCoroutine(PushLoginOrder(Global.Playerid(), Global.Password(), orderStr));
                }
            }
        }
    }
    /**
     * 把订单发送给服务器
     * */
    public IEnumerator PushLoginOrder(string id, string password, string orderStr)
    {
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("id", id);
        wwwForm.AddField("password", password);
        wwwForm.AddField("order", orderStr);
        wwwForm.AddField("operation", "recordorder");
        using (var www = UnityWebRequest.Post(NetWorkManager.Instance.url, wwwForm))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("Error:订单推送到服务器出现错误，error:" + www.error);
            }
            else
            {
                string ret_server = www.downloadHandler.text;
                ServerRet serverRet = new ServerRet();
                serverRet = JsonUtility.FromJson<ServerRet>(ret_server);
                if (serverRet.ret == 0)
                {
                    //移除本地订单
                    string orderStrTmp = serverRet.order;
                    JsonData orderTmp = JsonMapper.ToObject(orderStrTmp);
                    removeOrder("loginOrders", (string)orderTmp["orderid"]);
                    removeOrder("unLoginOrders", (string)orderTmp["orderid"]);
                    Debug.Log("保存订单成功! order:"+ orderStrTmp);
                }
                else
                {
                    Debug.Log("Error:保存订单失败!ret:" + serverRet.ret);
                }
            }
        }
    }

    //添加订单
    public void addOrder(JsonData order)
    {
        if (Global.IsLogin() == false)
        {
            JsonData orders = getOrdersByKey("unLoginOrders");
            orders.Add(order);
            var ordersStr = orders.ToJson();
            PlayerPrefs.SetString("unLoginOrders", ordersStr);
            PlayerPrefs.Save();
        }
        else
        {
            JsonData orders = getOrdersByKey("loginOrders");
            orders.Add(order);
            var ordersStr = orders.ToJson();
            PlayerPrefs.SetString("loginOrders", ordersStr);
            PlayerPrefs.Save();
        }
        PushLoginOrderNow();
    }

    //移除订单
    public void removeOrder(string key, string orderid)
    {
        JsonData orders = getOrdersByKey(key);
        for (int i = orders.Count-1; i >= 0; i--)
        {
            var order = orders[i];
            if (order.ContainsKey("orderid") && (string)order["orderid"] == orderid)
            {
                orders = jsonDataRemoveAt(orders, i);
                string ordersStr = orders.ToJson();
                PlayerPrefs.SetString(key, ordersStr);
                PlayerPrefs.Save();
                break;
            }
        }
    }

    //移除JsonData中指定index
    private JsonData jsonDataRemoveAt(JsonData jsonData, int index)
    {
        JsonData jsonDataNew = new JsonData();
        jsonDataNew.SetJsonType(JsonType.Array);
        for (int i = 0; i < jsonData.Count; i++)
        {
            if (i != index)
            {
                jsonDataNew.Add(jsonData[i]);
            }
        }
        return jsonDataNew;
    }

    //获得注册之前的订单
    private JsonData getOrdersByKey(string key)
    {
        JsonData orders = new JsonData();
        if (PlayerPrefs.HasKey(key))
        {
            string ordersStr = PlayerPrefs.GetString(key);
            orders = JsonMapper.ToObject(ordersStr);
        }
        orders.SetJsonType(JsonType.Array);
        return orders;
    }

    //获得注册之前的订单(string)
    public string getUnLoginOrdersStr()
    {
        JsonData orders = new JsonData();
        if (PlayerPrefs.HasKey("unLoginOrders"))
        {
            string ordersStr = PlayerPrefs.GetString("unLoginOrders");
            orders = JsonMapper.ToObject(ordersStr);
        }
        orders.SetJsonType(JsonType.Array);
        return orders.ToJson();
    }

    //清除注册之前的订单
    public void cleanUnLoginOrders()
    {
        if (PlayerPrefs.HasKey("unLoginOrders"))
        {
            PlayerPrefs.DeleteKey("unLoginOrders");
            PlayerPrefs.Save();
        }
    }

    //获得未推送给服务器的订单金币
    public int getUnPushGold()
    {
        int gold = 0;
        JsonData orders = getOrdersByKey("loginOrders");
        if (orders.Count > 0)
        {
            for (int i = 0; i < orders.Count; i++)
            {
                JsonData jsonData = orders[i];
                gold = gold + (int)jsonData["gold"];
            }
        }
        JsonData createOrders = getOrdersByKey("unLoginOrders");
        if (createOrders.Count > 0)
        {
            for (int i = 0; i < createOrders.Count; i++)
            {
                JsonData jsonData = createOrders[i];
                gold = gold + (int)jsonData["gold"];
            }
        }
        return gold;
    }
}
