using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PushManager : MonoBehaviour
{
    private static PushManager instance;

    public static PushManager Instance
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

    //推送数据间隔
    private int pushLoginOrderTime = 10*60;
    private int curPushLoginOrderPassTime = 0;

    //推送的key
    private List<string> pushKeys = new List<string>();

    void Awake()
    {
        instance = this;
        pushKeys.Add("addgold");
        pushKeys.Add("setgk");
    }
    
    void Update()
    {
        curPushLoginOrderPassTime++;
        if (curPushLoginOrderPassTime >= pushLoginOrderTime)
        {
            curPushLoginOrderPassTime = 0;
            PushOrderNow();
        }
    }

    private void PushOrderNow()
    {
        if (Global.IsLogin() && Global.Playerid() != null && Global.Password() != null)
        {
            for (int i = 0; i < pushKeys.Count; i++)
            {
                JsonData orders = getOrdersByKey(pushKeys[i]);
                if (orders.Count > 0)
                {
                    for (int j = 0; j < orders.Count; j++)
                    {
                        string orderStr = orders[j].ToJson();
                        StartCoroutine(PushOrder(Global.Playerid(), Global.Password(), pushKeys[i], orderStr));
                    }
                }
            }
        }
    }
    /**
     * 把数据发送给服务器
     * */
    public IEnumerator PushOrder(string id, string password, string key, string orderStr)
    {
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("id", id);
        wwwForm.AddField("password", password);
        wwwForm.AddField("key", key);
        wwwForm.AddField("order", orderStr);
        wwwForm.AddField("operation", "save");
        using (var www = UnityWebRequest.Post(NetWorkManager.Instance.url, wwwForm))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("Error:数据推送到服务器出现错误，error:" + www.error);
            }
            else
            {
                string ret_server = www.downloadHandler.text;
                ServerRet serverRet = new ServerRet();
                serverRet = JsonUtility.FromJson<ServerRet>(ret_server);
                if (serverRet.ret == 0)
                {
                    //移除本地数据
                    string orderStrTmp = serverRet.order;
                    JsonData orderTmp = JsonMapper.ToObject(orderStrTmp);
                    for (int i = 0; i < pushKeys.Count; i++)
                    {
                        if (hasOrder(pushKeys[i], (string)orderTmp["uid"]) == true)
                        {
                            removeOrder(pushKeys[i], (string)orderTmp["uid"]);
                            break;
                        }
                    }
                    Debug.Log("保存数据成功! order:" + orderStrTmp);
                }
                else if(serverRet.ret == 7)//重复保存
                {
                    //移除本地数据
                    string orderStrTmp = serverRet.order;
                    JsonData orderTmp = JsonMapper.ToObject(orderStrTmp);
                    for (int i = 0; i < pushKeys.Count; i++)
                    {
                        if (hasOrder(pushKeys[i], (string)orderTmp["uid"]) == true)
                        {
                            removeOrder(pushKeys[i], (string)orderTmp["uid"]);
                            break;
                        }
                    }
                    Debug.Log("重复保存! order:" + orderStrTmp);
                }
                else
                {
                    Debug.Log("Error:保存数据失败!ret:" + serverRet.ret);
                }
            }
        }
    }

    //添加数据
    public void addOrder(string key, JsonData order)
    {
        if (order.ContainsKey("uid") == false)
        {
            Debug.Log("Error, addOrder fail! order mast contain uid.");
            return;
        }
        JsonData orders = getOrdersByKey(key);
        orders.Add(order);
        var ordersStr = orders.ToJson();
        PlayerPrefs.SetString(key, ordersStr);
        PlayerPrefs.Save();
        
        PushOrderNow();
    }

    //移除数据
    public void removeOrder(string key, string uid)
    {
        JsonData orders = getOrdersByKey(key);
        for (int i = orders.Count - 1; i >= 0; i--)
        {
            var order = orders[i];
            if (order.ContainsKey("uid") && (string)order["uid"] == uid)
            {
                orders = jsonDataRemoveAt(orders, i);
                string ordersStr = orders.ToJson();
                PlayerPrefs.SetString(key, ordersStr);
                PlayerPrefs.Save();
                break;
            }
        }
    }

    //是否有数据
    public bool hasOrder(string key, string uid)
    {
        bool has = false;
        JsonData orders = getOrdersByKey(key);
        for (int i = orders.Count - 1; i >= 0; i--)
        {
            var order = orders[i];
            if (order.ContainsKey("uid") && (string)order["uid"] == uid)
            {
                has = true;
                break;
            }
        }
        return has;
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

    //获得注册之前的数据
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
    
    //清除注册之前的数据
    public void cleanOrdersByKey(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
        }
    }
}
