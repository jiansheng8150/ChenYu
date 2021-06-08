using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Praise : MonoBehaviour {
    //index=1
    public GameObject ImageGoodBg;
    public GameObject ImageGoodWord;
    //index=2
    public GameObject ImageGreatBg;
    public GameObject ImageGreatWord;

    //index=3
    public GameObject ImageAmazingBg1;
    public GameObject ImageAmazingBg2;
    public GameObject ImageAmazingBg3;
    public GameObject ImageAmazingWord;
    // Use this for initialization
    void Start () {
        
	}

    void OnEnable()
    {
        Invoke("hide", 1.70f);
    }
    // Update is called once per frame
    void Update () {
		
	}
    public void hide()
    {
        gameObject.SetActive(false);
    }

    /**
     * 显示编号（1或2或3）
     * */
    public void ShowIndex(int index)
    {
        ImageGoodBg.SetActive(false);
        ImageGoodWord.SetActive(false);
        ImageGreatBg.SetActive(false);
        ImageGreatWord.SetActive(false);
        ImageAmazingBg1.SetActive(false);
        ImageAmazingBg2.SetActive(false);
        ImageAmazingBg3.SetActive(false);
        ImageAmazingWord.SetActive(false);
        switch (index)
        {
            case 1:
                ImageGoodBg.SetActive(true);
                ImageGoodWord.SetActive(true);
                break;
            case 2:
                ImageGreatBg.SetActive(true);
                ImageGreatWord.SetActive(true);
                break;
            case 3:
                ImageAmazingBg1.SetActive(true);
                ImageAmazingBg2.SetActive(true);
                ImageAmazingBg3.SetActive(true);
                ImageAmazingWord.SetActive(true);
                break;
            default:
                ImageGoodBg.SetActive(true);
                ImageGoodWord.SetActive(true);
                break;
        }
    }
}
