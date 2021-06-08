using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class ServerRet
{
    //服务器返回结果（0表示成功，-1表示服务器返回数据格式不正确，其他表示失败）
    public int ret = -1;

    //错误信息
    public string msg;
    
    //用户金币数
    public int gold;

    //用户已通过的关卡
    public int gk;

    //用户名称
    public string name = "";

    //订单信息
    public string order = "";

    //新版本地址
    public string url;
}
