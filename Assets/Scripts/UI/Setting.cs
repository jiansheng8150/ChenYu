using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour {
    public GameObject ButtonOpen1;
    public GameObject ButtonClose1;
    public GameObject ButtonOpen2;
    public GameObject ButtonClose2;
    public GameObject ButtonOpen3;
    public GameObject ButtonClose3;
    // Use this for initialization
    void Start ()
    {
        ButtonOpen1.SetActive(Global.IsPlayMusic());
        ButtonClose1.SetActive(!Global.IsPlayMusic());
        ButtonOpen2.SetActive(Global.IsPlaySoundEffect());
        ButtonClose2.SetActive(!Global.IsPlaySoundEffect());
        ButtonOpen3.SetActive(Global.IsOpenNotification());
        ButtonClose3.SetActive(!Global.IsOpenNotification());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /**
     * 点击打开音乐
     * */
    public void ClickButtonOpen1()
    {
        AudioManager.Instance.PlayEffect("click");
        PlayerPrefs.SetString("IsPlayMusic", "false");
        AudioManager.Instance.PlayMusic(false);
        PlayerPrefs.Save();
        ButtonOpen1.SetActive(Global.IsPlayMusic());
        ButtonClose1.SetActive(!Global.IsPlayMusic());
    }

    /**
     * 点击关闭音乐
     * */
    public void ClickButtonClose1()
    {
        AudioManager.Instance.PlayEffect("click");
        PlayerPrefs.SetString("IsPlayMusic", "true");
        AudioManager.Instance.PlayMusic(true);
        PlayerPrefs.Save();
        ButtonOpen1.SetActive(Global.IsPlayMusic());
        ButtonClose1.SetActive(!Global.IsPlayMusic());
    }

    /**
     * 点击打开音效
     * */
    public void ClickButtonOpen2()
    {
        PlayerPrefs.SetString("IsPlaySoundEffect", "false");
        PlayerPrefs.Save();
        ButtonOpen2.SetActive(Global.IsPlaySoundEffect());
        ButtonClose2.SetActive(!Global.IsPlaySoundEffect());
    }

    /**
     * 点击关闭音效
     * */
    public void ClickButtonClose2()
    {
        PlayerPrefs.SetString("IsPlaySoundEffect", "true");
        AudioManager.Instance.PlayEffect("click");
        PlayerPrefs.Save();
        ButtonOpen2.SetActive(Global.IsPlaySoundEffect());
        ButtonClose2.SetActive(!Global.IsPlaySoundEffect());
    }

    /**
     * 点击打开通知
     * */
    public void ClickButtonOpen3()
    {
        AudioManager.Instance.PlayEffect("click");
        PlayerPrefs.SetString("IsOpenNotification", "false");
        PlayerPrefs.Save();
        ButtonOpen3.SetActive(Global.IsOpenNotification());
        ButtonClose3.SetActive(!Global.IsOpenNotification());
    }

    /**
     * 点击关闭通知
     * */
    public void ClickButtonClose3()
    {
        AudioManager.Instance.PlayEffect("click");
        PlayerPrefs.SetString("IsOpenNotification", "true");
        PlayerPrefs.Save();
        ButtonOpen3.SetActive(Global.IsOpenNotification());
        ButtonClose3.SetActive(!Global.IsOpenNotification());
    }

    /**
     * 点击关闭按钮
     * */
    public void ClickButtonClose()
    {
        AudioManager.Instance.PlayEffect("click");
        gameObject.SetActive(false);
    }

}
