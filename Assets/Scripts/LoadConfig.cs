using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadConfig : MonoBehaviour
{
    private static LoadConfig instance;

    //存储地址
    private string url;

    //数据
    //"北京":{ "kaishishijian":"00|00|00", "jieshushijian":"00|03|10", "zhuahuo":466, "xingju":65, "zengzhangbi1":4.8, "jiaohuodupin":5.02, "zengzhangbi2":25.5, "pohuoanjian":10, "shijianjian":5, "qujianjian":5, "biaoti":"北京市进度成果展现", "xiangxineirong":"2015 年打击数据\n今年以来，禁毒中队按照市局和分局统一部署，牵动局属各派出所明确责任"},
    public JsonData data;

    public static LoadConfig Instance
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

    // Use this for initialization
    private void Awake()
    {
        instance = this;

#if UNITY_EDITOR
        url = "file://"+Application.streamingAssetsPath + "/config.txt";
#else
        url = Application.streamingAssetsPath + "/config.txt";
#endif

        StartCoroutine(LoadAsset(url));
    }
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private IEnumerator LoadAsset(string url)
    {
        WWW www = new WWW(url);
        yield return new WaitForSeconds(0.01f);
        if (www.isDone)
        {
            //Debug.Log("www.text:"+ www.text);
            data = JsonMapper.ToObject(www.text);
            //data = JsonMapper.ToObject("[{\"curtext\": \"丽|天|质|生\",\"resulttext\": \"天|生|丽|质\",\"weight\": 1}]");
            data.SetJsonType(JsonType.Array);
            DataChange.OnPropChange("ConfigDone",null);
        }
    }
    public JsonData getGkConfig(int gk)
    {
        if (data == null)
        {
            Debug.Log("Error: data is null!");
            return null;
        }
        if (gk > 0 && gk <= data.Count)
        {
            return data[gk-1];
        }
        return null;
    }
}
