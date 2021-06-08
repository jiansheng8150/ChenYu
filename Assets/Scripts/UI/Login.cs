using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour {
    //邮件
    public InputField TextEmail;

    //密码
    public InputField TextPassword;

    //提示
    public Text TextNote;
    public GameObject TextNoteGo;

    // Use this for initialization
    void Start () {
        
    }
    void OnEnable()
    {
        Init();
    }

    /**
     * 初始化显示
     * */
    private void Init()
    {
        TextPassword.text = "";
        if (PlayerPrefs.HasKey("lastloginplayerid"))
        {
            TextEmail.text = PlayerPrefs.GetString("lastloginplayerid");
        }
        TextNoteGo.SetActive(false);
    }
    // Update is called once per frame
    void Update () {
		
	}

    /**
     * 点击登录按钮
     * */
     public void OnLoginButtonClick()
    {
        AudioManager.Instance.PlayEffect("click");
        if (DataChange.Haslistener("loginResult"))
        {
            return;
        }
        TextNoteGo.SetActive(false);
        if (TextEmail.text == null || TextEmail.text == "")
        {
            TextNote.text = "电子邮件不能为空";
            TextNoteGo.SetActive(true);
            return;
        }
        if (TextPassword.text == null || TextPassword.text == "")
        {
            TextNote.text = "密码不能为空";
            TextNoteGo.SetActive(true);
            return;
        }
        if (TextEmail.text.IndexOf("@") == -1)
        {
            TextNote.text = "电子邮件格式不正确";
            TextNoteGo.SetActive(true);
            return;
        }
        //监听服务器返回结果
        DataChange.Addlistener("loginResult", OnLoginResult);
        //向服务器请求登录
        StartCoroutine(NetWorkManager.Instance.LoginUser(TextEmail.text, TextPassword.text));
    }

    /**
     * 登录返回结果
     * */
    private void OnLoginResult(object value)
    {
        if (gameObject.activeSelf)
        {
            if (value != null)
            {
                switch ((int)value)
                {
                    case 0:
                        gameObject.SetActive(false);
                        break;
                    case 1:
                        TextNote.text = "电子邮件不存在";
                        TextNoteGo.SetActive(true);
                        break;
                    case 2:
                        TextNote.text = "密码错误";
                        TextNoteGo.SetActive(true);
                        break;
                    default:
                        TextNote.text = "登录失败";
                        TextNoteGo.SetActive(true);
                        break;
                }
            }
            else
            {
                TextNote.text = "网络连接错误";
                TextNoteGo.SetActive(true);
            }
        }
        DataChange.Removelistener("loginResult", OnLoginResult);
    }

    /**
     * 点击背景关闭界面
     * */
    public void OnBgClick()
    {
        AudioManager.Instance.PlayEffect("click");
        PanelAnimator panelAnimator = gameObject.GetComponent<PanelAnimator>();
        if (panelAnimator)
        {
            panelAnimator.Hide();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    //隐藏动画结束
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
