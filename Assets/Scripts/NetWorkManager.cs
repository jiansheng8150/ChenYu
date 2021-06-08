using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetWorkManager : MonoBehaviour {
    private static NetWorkManager instance;

    public static NetWorkManager Instance
    {
        get
        {
            return instance;
        }
    }

    //服务器地址
    //url = "http://47.95.228.156:90";    // 云服务器地址
    //url = "http://192.168.199.121:1524";// 本地服务器地址
    public string url = "";   // 在inspector窗口中设置

    //当前版本号
    private int versionNum = 100;

    void Awake()
    {
        instance = this;

        //读取本地数据
        UserData data;
        if (PlayerPrefs.HasKey("data"))
        {
            try { 
                string userDataStr = PlayerPrefs.GetString("data");
                data = JsonUtility.FromJson<UserData>(userDataStr);
            }
            catch
            {
                data = new UserData();
            }
        }
        else
        {
            data = new UserData();
        }
        UserData.Instance = data;

        //UserData.Instance.Gk = 4;// 测试数据
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();

        if (PlayerPrefs.HasKey("lastTimeGetReward") == false)
        {
            DateTime dt0 = new DateTime(1970, 1, 1, 8, 0, 0);
            TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - dt0.Ticks);
            PlayerPrefs.SetString("lastTimeGetReward", ts.Ticks.ToString());
            PlayerPrefs.Save();
        }
        StartCoroutine(CheckVersion());
    }

    // Use this for initialization
    void Start() {
        //测试
        //StartCoroutine(CreateUser("gg002@163.com", "ff", "gg"));
        //StartCoroutine(LoginUser("gg002@163.com", "ff"));
        //StartCoroutine(SaveServerData("gg002@163.com", "ff", "addgold", 10));
        //StartCoroutine(SaveServerData("gg002@163.com", "ff", "setgk", 5));
        //StartCoroutine(GetServerData("gg002@163.com", "ff"));
        //JsonData golddata = new JsonData();
        //golddata["gold"] = 5000;
        //golddata["uid"] = "gold_1";
        //PushManager.Instance.addOrder("addgold", golddata);

        //JsonData golddata2 = new JsonData();
        //golddata2["gk"] = 23;
        //golddata2["uid"] = StringTool.getUid();
        //PushManager.Instance.addOrder("setgk", golddata2);

        //UserData.Instance.addGold(1000);
        //UserData.Instance.setGk(356);

        //CheckReward();
    }

    /**
     * 检测版本更新
     * */
    IEnumerator CheckVersion()
    {
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("version", versionNum);
        wwwForm.AddField("operation", "checkversion");
        UnityWebRequest www = UnityWebRequest.Post(url, wwwForm);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            if (Global.IsLogin() == true && PlayerPrefs.HasKey("playerid") && PlayerPrefs.HasKey("password"))
            {
                Global.playerid = PlayerPrefs.GetString("playerid");
                Global.password = PlayerPrefs.GetString("password");

                //请求最新数据（其他设备登录该账号，数据需要同步）
                StartCoroutine(GetServerData(Global.playerid, Global.password));
            }
            CheckReward();
            Debug.Log("Error:检测版本出现错误，error:" + www.error);
        }
        else
        {
            string ret_server = www.downloadHandler.text;
            ServerRet serverRet = new ServerRet();
            serverRet = JsonUtility.FromJson<ServerRet>(ret_server);
            if (serverRet.ret == 0)
            {
                if (Global.IsLogin() == true && PlayerPrefs.HasKey("playerid") && PlayerPrefs.HasKey("password"))
                {
                    Global.playerid = PlayerPrefs.GetString("playerid");
                    Global.password = PlayerPrefs.GetString("password");

                    //请求最新数据（其他设备登录该账号，数据需要同步）
                    StartCoroutine(GetServerData(Global.playerid, Global.password));
                }
                CheckReward();
                Debug.Log("不需要更新!");
            }
            else
            {
                if (serverRet.url != null && serverRet.url != "")
                {
                    GameObject go = UIManager.Instance.GetGameObjectByName("PanelNewVersion");
                    NewVersion newVersion = go.GetComponent<NewVersion>();
                    if (newVersion != null)
                    {
                        newVersion.SetUrl(serverRet.url);
                    }
                }
                UIManager.Instance.ShowPanel("PanelNewVersion", false);
                Debug.Log("需要更新版本!");
            }
        }
    }

    /**
     * 检测是否打开奖励界面
     * */
    private void CheckReward()
    {
        if (UserData.Instance.Gk <= 5)
        {
            return;
        }
        //Debug.Log("reward open!");
        string dateVal = PlayerPrefs.GetString("lastTimeGetReward");
        TimeSpan time = new TimeSpan(long.Parse(dateVal));
        DateTime dt0 = new DateTime(1970, 1, 1, 8, 0, 0);
        TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - dt0.Ticks);
        if ((int)time.TotalDays != (int)ts.TotalDays)
        {
            PlayerPrefs.SetString("lastTimeGetReward", ts.Ticks.ToString());
            PlayerPrefs.Save();
            UIManager.Instance.ShowPanel("PanelReward", false);
        }
    }

    /**
     * 创建用户
     * */
    public IEnumerator CreateUser(string id, string password, string name)
    {
        string ordersStr = OrderManager.Instance.getUnLoginOrdersStr();
        Debug.Log("ordersStr:" + ordersStr);
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("id", id);
        wwwForm.AddField("password", password);
        wwwForm.AddField("name", name);
        wwwForm.AddField("gold", UserData.Instance.Gold);
        wwwForm.AddField("gk", UserData.Instance.Gk);
        wwwForm.AddField("orders", ordersStr);
        wwwForm.AddField("operation", "create");
        UnityWebRequest www = UnityWebRequest.Post(url, wwwForm);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Error:创建用户出现错误，error:" + www.error);
            DataChange.OnPropChange("registerResult", null);
        }
        else
        {
            string ret_server = www.downloadHandler.text;
            ServerRet serverRet = new ServerRet();
            serverRet = JsonUtility.FromJson<ServerRet>(ret_server);
            if (serverRet.ret == 0)
            {
                //保存用户id和密码
                Global.playerid = id;
                Global.password = password;
                PlayerPrefs.SetString("playerid", id);
                PlayerPrefs.SetString("password", password);
                PlayerPrefs.SetString("name", name);
                PlayerPrefs.SetString("isLogin", "true");
                PlayerPrefs.SetString("lastloginplayerid", id);
                PlayerPrefs.Save();
                OrderManager.Instance.cleanUnLoginOrders();
                DataChange.OnPropChange("loginChange", null);
                DataChange.OnPropChange("userDataChange", null);
                Debug.Log("用户创建成功!");
            }
            else
            {
                Debug.Log("Error:用户创建失败!ret:" + serverRet.ret);
            }
            DataChange.OnPropChange("registerResult", serverRet.ret);
        }
    }

    /**
     * 用户登录
     * */
    public IEnumerator LoginUser(string id, string password)
    {
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("id", id);
        wwwForm.AddField("password", password);
        wwwForm.AddField("operation", "login");
        UnityWebRequest www = UnityWebRequest.Post(url, wwwForm);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Error:用户登录出现错误，error:" + www.error);
            DataChange.OnPropChange("loginResult", null);
        }
        else
        {
            string ret_server = www.downloadHandler.text;
            ServerRet serverRet = new ServerRet();
            serverRet = JsonUtility.FromJson<ServerRet>(ret_server);
            if (serverRet.ret == 0)
            {
                //保存用户id和密码
                Global.playerid = id;
                Global.password = password;
                PlayerPrefs.SetString("playerid", id);
                PlayerPrefs.SetString("password", password);
                PlayerPrefs.SetString("name", serverRet.name);
                PlayerPrefs.SetString("isLogin", "true");
                PlayerPrefs.SetString("lastloginplayerid", id);
                PlayerPrefs.Save();

                //刷新显示
                UserData.Instance.Gold = serverRet.gold;
                UserData.Instance.Gk = serverRet.gk;
                UserData.Instance.Name = serverRet.name;

                DataChange.OnPropChange("loginChange", null);
                DataChange.OnPropChange("userDataChange", null);
                Debug.Log("用户登录成功!    gold:" + UserData.Instance.Gold + "    gk:" + UserData.Instance.Gk);
            }
            else
            {
                Debug.Log("Error:用户登录失败!ret:" + serverRet.ret);
            }
            DataChange.OnPropChange("loginResult", serverRet.ret);
        }
    }
    
    /**
     * 获取退出登录
     * */
    public void UnLogin()
    {
        if (Global.IsLogin() && Global.playerid != null)
        {
            PlayerPrefs.SetString("isLogin", "false");
            PlayerPrefs.Save();
            DataChange.OnPropChange("loginChange", null);
        }
    }

    /**
     * 获取用户数据
     * */
    public IEnumerator GetServerData(string id, string password)
    {
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("id", id);
        wwwForm.AddField("password", password);
        wwwForm.AddField("operation", "get");
        UnityWebRequest www = UnityWebRequest.Post(url, wwwForm);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Error:获取用户数据出现错误,error:" + www.error);
        }
        else
        {
            string data_server = www.downloadHandler.text;
            ServerRet serverRet = new ServerRet();
            serverRet = JsonUtility.FromJson<ServerRet>(data_server);
            if (serverRet.ret == 0)
            {
                if (Global.IsLogin() && Global.Playerid() != null && Global.Password() != null && Global.Playerid() == id) // 用户处于登录状态
                {
                    UserData.Instance.Gold = serverRet.gold;
                    UserData.Instance.Gk = serverRet.gk;
                    UserData.Instance.Name = serverRet.name;
                    DataChange.OnPropChange("userDataChange", null);
                    Debug.Log("获取数据成功!    gold:"+ UserData.Instance.Gold+ "    gk:"+ UserData.Instance.Gk);
                }
            }
            else
            {
                Debug.Log("Error:获取用户数据失败！msg:" + serverRet.msg);
            }
        }
    }

    /**
     * 保存用户数据
     * */
    public void SaveServerData1(string id, string password, string f, int num)
    {
        StartCoroutine(SaveServerData(id, password, f, num));
    }
    /**
     * 保存用户数据
     * */
    public IEnumerator SaveServerData(string id, string password, string f, int num)
    {
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("id", id);
        wwwForm.AddField("password", password);
        wwwForm.AddField("f", f);
        if (f == "addgold")
        {
            wwwForm.AddField("gold", num);
        }
        else if(f == "setgk")
        {
            wwwForm.AddField("gk", num);
        }
        wwwForm.AddField("operation", "save");
        UnityWebRequest www = UnityWebRequest.Post(url, wwwForm);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Error:保存用户数据出现错误，error:"+www.error);
        }
        else
        {
            string ret_server = www.downloadHandler.text;
            ServerRet serverRet = new ServerRet();
            serverRet = JsonUtility.FromJson<ServerRet>(ret_server);
            if (serverRet.ret == 0)
            {
                Debug.Log("保存用户数据成功!");
            }
            else 
            {
                Debug.Log("Error:保存用户数据失败，msg"+serverRet.msg);
            }
        }
    }

}
