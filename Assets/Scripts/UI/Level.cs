using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    /**
     * 点击回主界面按钮
     * */
    public void ClickButtonMain()
    {
        AudioManager.Instance.PlayEffect("click");
        UIManager.Instance.SwitchPanel("PanelMain");
    }

    /**
     * 点击回游戏按钮
     * */
    public void ClickButtonBack()
    {
        AudioManager.Instance.PlayEffect("click");
        UIManager.Instance.SwitchPanel("PanelGame");
    }
}
