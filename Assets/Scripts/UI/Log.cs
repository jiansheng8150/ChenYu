using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Log: MonoBehaviour {

    public Text TextLog;

    // Use this for initialization
    void Start () {
        
	}
    // Update is called once per frame
    void Update () {
		
	}

    /**
     * 显示log
     * */
    public void ShowLog(string text)
    {
        TextLog.text = TextLog.text + "\n" + text;
    }

    /**
     * 点击
     * */
    public void OnButtonClick()
    {
        UIManager.Instance.HidePanel("PanelLog");
    }
}
