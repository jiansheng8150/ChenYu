using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterMain : MonoBehaviour
{
    //注册按钮
    public GameObject ButtonRegiter;

    //登录按钮
    public GameObject ButtonLogin;

    //退出按钮
    public GameObject ButtonQuit;

    //提示文本
    public Text TextNote;
    private void Awake()
    {
        DataChange.Addlistener("loginChange", OnLoginChange);
    }

    /**
     * 登录状态改变
     * */
    private void OnLoginChange(object value)
    {
        RefreshShow();
    }

    // Use this for initialization
    void Start () {
        
	}

    void OnEnable()
    {
        RefreshShow();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /**
     * 刷新显示
     * */
    private void RefreshShow()
    {
        if (Global.IsLogin() && Global.playerid != null)
        {
            TextNote.text = "当前帐号:" + Global.playerid;
            ButtonRegiter.SetActive(false);
            ButtonLogin.SetActive(false);
            ButtonQuit.SetActive(true);
        }
        else
        {
            TextNote.text = "登录帐号，立即同步您的数据";
            ButtonRegiter.SetActive(true);
            ButtonLogin.SetActive(true);
            ButtonQuit.SetActive(false);
        }
    }

    /**
     * 点击注册按钮
     */
    public void OnRegisterButtonClick()
    {
        AudioManager.Instance.PlayEffect("click");
        UIManager.Instance.HidePanel("CanvasLoginMain");
        UIManager.Instance.ShowPanel("CanvasUser", false);
        UIManager.Instance.ShowPanel("CanvasRegister", false);
    }

    /**
     * 点击登录按钮
     */
    public void OnLoginButtonClick()
    {
        AudioManager.Instance.PlayEffect("click");
        UIManager.Instance.HidePanel("CanvasLoginMain");
        UIManager.Instance.ShowPanel("CanvasUser", false);
        UIManager.Instance.ShowPanel("CanvasLogin", false);
    }

    /**
     * 点击退出按钮
     */
    public void OnQuitButtonClick()
    {
        AudioManager.Instance.PlayEffect("click");
        CallIOS.ShowSelectTitleDialog("unlogin", "提示", "退出 " + Global.playerid + " 的帐户？");
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
}
