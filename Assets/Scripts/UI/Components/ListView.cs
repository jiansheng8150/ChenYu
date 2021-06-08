using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListView : MonoBehaviour {
    //容器
    public Scrollbar scrollbar;
    public RectTransform ScrollRectRransform;
    public RectTransform content;
    public Transform contentts;
    private float contentHeight;


    //子对象高度
    private int childHeight = 160;

    //显示的内容
    private List<int> listData = new List<int>();

    //总数量
    private int totalNum = 0;

    //显示数量
    private int showNum = 0;

    //内容数量
    private int dataNum = 500;
    private float initItemNum;


    // Use this for initialization
    
    void Awake()
    {
        init();
        DataChange.Addlistener("ConfigDone",OnConfigDone);
    }

    private void OnConfigDone(object value)
    {
        Start();
    }

    void OnEnable()
    {
        OnValueChange(new Vector2(0.5f, 1f));
    }
    private void init()
    {
        if (LoadConfig.Instance.data == null)
        {
            return;
        }
        dataNum = LoadConfig.Instance.data.Count;
        initItemNum = ScrollRectRransform.rect.height / childHeight;
        for (int i = 0; i < dataNum; i++)
        {
            listData.Add(i);
        }
        totalNum = contentts.childCount;
        showNum = (int)ScrollRectRransform.rect.height / childHeight + 1;
        contentHeight = listData.Count * childHeight;
        content.sizeDelta = new Vector2(content.rect.width, contentHeight);
    }
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {

    }

    /**
     * 滚到指定项
     * */
    public void ScrollToIndex(int index)
    {
        index = index - 1;
        float value = 1 - index / (dataNum - initItemNum);
        value = Mathf.Clamp(value,0.00000001f,1);
        scrollbar.value = value;
    }

    public void OnValueChange(Vector2 value)
    {
        RefreshShow(value);
    }

    private void RefreshShow(Vector2 value)
    {
        float height = content.localPosition.y;
        int startIndex = 0;
        startIndex = (int)height / childHeight;
        startIndex = startIndex - (totalNum - showNum) / 2;
        if (startIndex<0)
        {
            startIndex = 0;
        }
        if (startIndex > listData.Count - totalNum)
        {
            startIndex = listData.Count - totalNum;
        }
        for (int i = startIndex; i < startIndex + totalNum; i++)
        {
            if (i >= 0 && i < listData.Count)
            {
                Transform ts = contentts.GetChild(i- startIndex);
                ts.localPosition = new Vector3(ts.localPosition.x, -i * childHeight, ts.localPosition.z);
                PanelLevelSmall panelLevelSmall = ts.GetComponent<PanelLevelSmall>();
                if (panelLevelSmall != null)
                {
                    panelLevelSmall.SetGk(i + 1);
                }
            }
        }
    }
}
