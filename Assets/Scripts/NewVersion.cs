using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewVersion : MonoBehaviour {

    private string url = "https://play.google.com/store/apps/details?id=com.jianshenglin.connectidiom";
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetUrl(string value)
    {
        url = value;
    }

    public void ClickButtonUpdate()
    {
        Debug.Log("open url");
        Application.OpenURL(url);
    }
}
