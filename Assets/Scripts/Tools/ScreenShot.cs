using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShot : MonoBehaviour
{
    private static ScreenShot instance;
    
    public static ScreenShot Instance
    {
        get
        {
            return instance;
        }
        
        set
        {
            instance = value;
        }
    }
    public delegate void FuncScreenShotCallBack(string fullpath);

    //截屏的照相机
    public Camera CameraScreenShot;

    //UI
    public GameObject CanvasScreenShot;

    //each UI
    public GameObject CanvasShareEach;
    public RectTransform ImageBgEach;

    //gold UI
    public GameObject CanvasShareGold;
    public RectTransform ImageBgGold;

    //each 显示文字
    public Text TextTitleEach;

    //gold 显示文字
    public Text TextTitleGold;

    //宽高比
    private float RatioW;

    void Awake()
    {
        instance = this;
        CanvasScreenShot.SetActive(false);
        CanvasShareEach.SetActive(false);
        CanvasShareGold.SetActive(false);
        CameraScreenShot.enabled = false;

        RatioW = (float)Screen.width / 1080;
    }

    /**
     * 分享图片
     * */
    public void StartShot(string type, string text, FuncScreenShotCallBack callback)
    {
        CanvasScreenShot.SetActive(true);
        CanvasShareEach.SetActive(false);
        CanvasShareGold.SetActive(false);
        CameraScreenShot.enabled = true;
        Rect rect;
        //截屏
        if (type == "each")
        {
            TextTitleEach.text = text;
            CanvasShareEach.SetActive(true);
            rect = new Rect(0, (Screen.height-ImageBgEach.rect.height* RatioW) /2, ImageBgEach.rect.width * RatioW, ImageBgEach.rect.height * RatioW);
        }
        else if (type == "gold")
        {
            TextTitleGold.text = text;
            CanvasShareGold.SetActive(true);
            rect = new Rect(0, (Screen.height - ImageBgGold.rect.height * RatioW) / 2, ImageBgGold.rect.width * RatioW, ImageBgGold.rect.height * RatioW);
        }
        else
        {
            rect = new Rect(0, 0, Screen.width, Screen.height);
        }
        StartCoroutine(CaptureScreenshot(rect, callback));
    }

    IEnumerator CaptureScreenshot(Rect rect, FuncScreenShotCallBack callback)
    {
        string filename = "screenShot";
        yield return new WaitForEndOfFrame();

        RenderTexture rt = new RenderTexture((int)Screen.width, (int)Screen.height, 16);
        CameraScreenShot.targetTexture = rt;
        CameraScreenShot.Render();
        RenderTexture.active = rt;

        // 先创建一个的空纹理，大小可根据实现需要来设置
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        // 读取屏幕像素信息并存储为纹理数据
        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();
        
        CameraScreenShot.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        // 然后将这些纹理数据，成一个png图片文件
        byte[] bytes = screenShot.EncodeToJPG();
        string fullpath = Application.persistentDataPath + "/" + filename + ".jpg";
        System.IO.File.WriteAllBytes(fullpath, bytes);
        Debug.Log(string.Format("截屏了一张图片: {0}", fullpath));

        CanvasScreenShot.SetActive(false);
        CanvasShareEach.SetActive(false);
        CanvasShareGold.SetActive(false);
        CameraScreenShot.enabled = false;

        callback.Invoke(fullpath);
    }

}
