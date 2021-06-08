using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waiting : MonoBehaviour {

    //waiting图片
    public RectTransform ImageWaiting;

    //时间间隔
    private int interval = 4;
    private int curTimes = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (curTimes >= interval)
        {
            curTimes = 0;
            ImageWaiting.eulerAngles = new Vector3(ImageWaiting.eulerAngles.x, ImageWaiting.eulerAngles.y, ImageWaiting.eulerAngles.z - 30);
        }
        curTimes++;
    }
}
