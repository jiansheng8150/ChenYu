using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DataChange {
    public delegate void FunctionCallBack(object value);

    private static Dictionary<string, FunctionCallBack> _callbacks = new Dictionary<string, FunctionCallBack>();

    /**
     * 添加监听
     * */
    public static void Addlistener(string propName, FunctionCallBack callback)
    {
        if (_callbacks.ContainsKey(propName))
        {
            _callbacks[propName] += callback;
        }
        else
        {
            _callbacks[propName] = callback;
        }
    }

    /**
     * 移除监听
     * */
    public static void Removelistener(string propName, FunctionCallBack callback)
    {
        if (_callbacks.ContainsKey(propName))
        {
            _callbacks[propName] -= callback;
            if (_callbacks[propName] == null)
            {
                _callbacks.Remove(propName);
            }
        }
    }

    /**
     * 是否有监听
     * */
    public static bool Haslistener(string propName)
    {
        if (_callbacks.ContainsKey(propName) && _callbacks[propName] != null)
        {
            return true;
        }
        return false;
    }

    /**
     * 数据改变
     * */
    public static void OnPropChange(string propName, object value)
    {
        if (_callbacks.ContainsKey(propName))
        {
            _callbacks[propName].Invoke(value);
        }
    }
}
