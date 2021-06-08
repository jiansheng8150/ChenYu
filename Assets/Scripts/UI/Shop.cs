using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /**
     * 点击购买按钮
     * */
    public void ClickButtonPrice(int index)
    {
        AudioManager.Instance.PlayEffect("click");
        if (index == 1)
        {
            UIManager.Instance.ShowPanel("PanelWaitingText", false);
            CallIOS.BuyProduct(Global.PRODUCTID1);
        }
        else if (index == 2)
        {
            UIManager.Instance.ShowPanel("PanelWaitingText", false);
            CallIOS.BuyProduct(Global.PRODUCTID2);
        }
        else if (index == 3)
        {
            UIManager.Instance.ShowPanel("PanelWaitingText", false);
            CallIOS.BuyProduct(Global.PRODUCTID3);
        }
        else
        {
            Debug.Log("Error: index is wrong! index:" + index);
        }
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
