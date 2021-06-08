using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelWord : MonoBehaviour {
    public Image ImageWord;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setColorAlpha(float value)
    {
        ImageWord.color = new Color(ImageWord.color.r, ImageWord.color.g, ImageWord.color.b, value);
    }
}
