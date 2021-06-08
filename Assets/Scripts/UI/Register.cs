using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    //名称
    public InputField TextName;

    //邮件
    public InputField TextEmail;

    //密码
    public InputField TextPassword1;

    //确认密码
    public InputField TextPassword2;

    //提示
    public Text TextNote;
    public GameObject TextNoteGo;

    //选中条款
    public Toggle ToggleAgreement;

    // Use this for initialization
    void Start () {
        TextNoteGo.SetActive(false);
    }
    void OnEnable()
    {
        TextName.text = "";
        TextEmail.text = "";
        TextPassword1.text = "";
        TextPassword2.text = "";
    }
    // Update is called once per frame
    void Update () {
		
	}

    /**
     * 点击注册按钮
     * */
     public void OnRegisterButtonClick()
    {
        AudioManager.Instance.PlayEffect("click");
        if (DataChange.Haslistener("registerResult"))
        {
            return;
        }
        TextNoteGo.SetActive(false);
        if (TextName.text == null || TextName.text == "")
        {
            TextNote.text = "名称不能为空";
            TextNoteGo.SetActive(true);
            return;
        }
        if (TextEmail.text == null || TextEmail.text == "")
        {
            TextNote.text = "电子邮件不能为空";
            TextNoteGo.SetActive(true);
            return;
        }
        if (TextPassword1.text == null || TextPassword1.text == "")
        {
            TextNote.text = "密码不能为空";
            TextNoteGo.SetActive(true);
            return;
        }
        if (TextName.text.Length > 20)
        {
            TextNote.text = "名称不能超过20个字符";
            TextNoteGo.SetActive(true);
            return;
        }
        if (TextEmail.text.Length > 100)
        {
            TextNote.text = "电子邮件不能超过100个字符";
            TextNoteGo.SetActive(true);
            return;
        }
        if (TextPassword1.text.Length > 100)
        {
            TextNote.text = "密码不能超过100个字符";
            TextNoteGo.SetActive(true);
            return;
        }
        if (TextEmail.text.IndexOf("@") == -1)
        {
            TextNote.text = "电子邮件格式不正确";
            TextNoteGo.SetActive(true);
            return;
        }
        if (TextPassword1.text != TextPassword2.text)
        {
            TextNote.text = "密码不一致";
            TextNoteGo.SetActive(true);
            return;
        }
        if (ToggleAgreement.isOn == false)
        {
            TextNote.text = "必须勾选同意服务条款和隐私权政策";
            TextNoteGo.SetActive(true);
            return;
        }
        //监听服务器返回结果
        DataChange.Addlistener("registerResult", OnRegisterResult);
        //向服务器发送注册用户请求
        StartCoroutine(NetWorkManager.Instance.CreateUser(TextEmail.text, TextPassword1.text, TextName.text));
    }

    /**
     * 注册用户返回结果
     * */
    private void OnRegisterResult(object value)
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
                    case 2:
                        TextNote.text = "电子邮件已被占用";
                        TextNoteGo.SetActive(true);
                        break;
                    default:
                        TextNote.text = "注册失败";
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
        DataChange.Removelistener("registerResult", OnRegisterResult);
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
