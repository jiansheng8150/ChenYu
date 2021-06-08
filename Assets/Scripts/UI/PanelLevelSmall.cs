using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelLevelSmall : MonoBehaviour {
    public Text TextTitle;
    public GameObject ImageClear1;
    public GameObject ImageClear2;
    public GameObject ImageCur;
    public GameObject ImageLock;

    //Game
    public Game game;

    //关卡
    private int gk=-1;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetGk(int gk)
    {
        this.gk = gk;
        SetTitle("关卡"+gk);
        if (gk <= UserData.Instance.Gk)
        {
            SetState(2);
        }
        else if (gk == UserData.Instance.Gk + 1)
        {
            SetState(1);
        }
        else if (gk > UserData.Instance.Gk + 1)
        {
            SetState(3);
        }
    }

    private void SetTitle(string title)
    {
        TextTitle.text = title;
    }

    /**
     * 设置通关状态（1表示未通过，2表示已通过，3表示未解锁）
     * */
    private void SetState(int state)
    {
        ImageClear1.SetActive(false);
        ImageClear2.SetActive(false);
        ImageCur.SetActive(false);
        ImageLock.SetActive(false);
        if (state == 1)
        {
            ImageCur.SetActive(true);
        }
        else if (state == 2)
        {
            ImageClear1.SetActive(true);
            ImageClear2.SetActive(true);
        }
        else if (state == 3)
        {
            ImageLock.SetActive(true);
        }
    }

    /**
     * 点击进入
     * */
    public void ClickIn()
    {
        if (gk != -1 && gk <= UserData.Instance.Gk+1)
        {
            AudioManager.Instance.PlayEffect("click");
            UIManager.Instance.ShowPanel("PanelGame", true);
            game.setGkShow(gk);
        }
    }
}
