using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reward : MonoBehaviour {
    //获得的金币
    private int getGold = 0;

    //数字
    public Image ImageNum1;
    public Image ImageNum2;
    public Image ImageNum3;
    // Use this for initialization
    void Start () {
        

    }
    void OnEnable()
    {
        System.Random random = new System.Random();
        getGold = random.Next(81, 200);
        int num1 = (int)getGold / 100;
        int num2 = (int)(getGold % 100) / 10;
        int num3 = (int)getGold % 10;
        ImageNum1.overrideSprite = Resources.Load("letou_" + num1, typeof(Sprite)) as Sprite;
        ImageNum2.overrideSprite = Resources.Load("letou_" + num2, typeof(Sprite)) as Sprite;
        ImageNum3.overrideSprite = Resources.Load("letou_" + num3, typeof(Sprite)) as Sprite;
    }
    // Update is called once per frame
    void Update () {
		
	}

    /**
     * 点击领取按钮
     * */
    public void ClickButtonGet()
    {
        AudioManager.Instance.PlayEffect("click");
        gameObject.SetActive(false);
        UserData.Instance.addGold(getGold);
    }

}
