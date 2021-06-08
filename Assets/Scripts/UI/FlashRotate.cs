using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashRotate : MonoBehaviour
{

    public GameObject ImageFlash1;
    public GameObject ImageFlash2;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        ImageFlash1.transform.Rotate(0,0,1.0f);
        ImageFlash2.transform.Rotate(0,0,1.2f);

    }
}
