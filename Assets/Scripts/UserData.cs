using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class UserData
{
    private static UserData instance;

    public static UserData Instance // 在NetWorkManager中赋值
    {
        get
        {
            return instance;
        }

        set
        {
            instance = value;
            DataChange.OnPropChange("gold", instance.gold);
            DataChange.OnPropChange("gk", instance.gk);
            DataChange.OnPropChange("name", instance.name);
        }
    }

    //用户金币
    public int gold = 0;

    //当前已经通过的关卡(关卡从1开始，当gk=1时，表示通过了第1关)
    public int gk = 0;

    //玩家名称
    public string name = "";

    public int Gold
    {
        get
        {
            return gold;
        }

        set
        {
            gold = value;
            Save();
            DataChange.OnPropChange("gold", instance.gold);
        }
    }
    
    public int Gk
    {
        get
        {
            return gk;
        }

        set
        {
            gk = value;
            Save();
            DataChange.OnPropChange("gk", instance.gk);
        }
    }

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
            Save();
            DataChange.OnPropChange("name", instance.name);
        }
    }

    void Awake()
    {
        instance = this;
    }
    /**
     * 转成json字符串（只有public变量才会转）
     * */
    override public string ToString()
    {
        return JsonUtility.ToJson(this);
    }
    
    /**
     * 数据保存到本地
     * */
    public void Save()
    {
        PlayerPrefs.SetString("data", this.ToString());
        PlayerPrefs.Save();
    }

    /**
     * 添加金币，支持负数
     * */
    public void addGold(int value)
    {
        Gold += value;
        JsonData golddata = new JsonData();
        golddata["gold"] = value;
        golddata["uid"] = StringTool.getUid();
        PushManager.Instance.addOrder("addgold", golddata);
    }

    /**
     * 设置关卡
     * */
    public void setGk(int value)
    {
        if (value > Gk)
        {
            Gk = value;
            JsonData gkdata = new JsonData();
            gkdata["gk"] = value;
            gkdata["uid"] = StringTool.getUid();
            PushManager.Instance.addOrder("setgk", gkdata);
        }
        else
        {
            Debug.Log("Error:save gk is lower than now!");
        }
    }
}
