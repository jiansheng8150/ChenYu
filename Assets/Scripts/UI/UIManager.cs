using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            return _instance;
        }
    }
    
    //所有界面（不包含主界面）
    private List<string> panelNames = new List<string>();
    public Dictionary<string, GameObject> panels = new Dictionary<string, GameObject>();

    //打开着的界面
    private List<string> openPanels = new List<string>();

    void Awake()
    {
        _instance = this;
        panelNames.Add("PanelMain");
        panelNames.Add("PanelGame");
        panelNames.Add("PanelPraise");
        panelNames.Add("PanelWin");
        panelNames.Add("PanelLevel");
        panelNames.Add("PanelShop");
        panelNames.Add("PanelWaitingText");
        panelNames.Add("PanelSetting");
        panelNames.Add("PanelReward");
        panelNames.Add("CanvasUser");
        panelNames.Add("CanvasLoginMain");
        panelNames.Add("CanvasRegister");
        panelNames.Add("CanvasLogin");
        panelNames.Add("PanelLoading");
        panelNames.Add("PanelNewVersion");
        panelNames.Add("PanelWaitingImage");
        panelNames.Add("PanelLog");

        foreach (string panelName in panelNames)
        {
            Transform goTransform = FindGameObject(transform, panelName);
            if (goTransform != null)
            {
                panels.Add(panelName, goTransform.gameObject);
            }
            else
            {
                Debug.Log("Error:没有找到对象，panelName:" + panelName);
            }
        }


    }

    void Update()
    {
        foreach (var item in panels)
        {
            if (openPanels.Contains(item.Key) && item.Value.activeSelf == false)
            {
                openPanels.Remove(item.Key);
            }
            if (openPanels.Contains(item.Key) == false && item.Value.activeSelf == true)
            {
                openPanels.Add(item.Key);
            }
        }
    }
    private Transform FindGameObject(Transform parent, string panelName)
    {
        Transform go = parent.Find(panelName);
        if (go)
        {
            return go;
        }
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform temp = parent.GetChild(i);
            go = FindGameObject(temp, panelName);
            if (go)
            {
                return go;
            }
        }
        return null;
    }
    /**
     * 显示界面(动画切换)
     * */
    public void SwitchPanel(string panelName, bool hideAll = true, bool animation = true)
    {
        if (animation)
        {
            ShowPanel(panelName, hideAll);
        }
        else
        {
            ShowPanel(panelName, hideAll);
        }
    }

    /**
     * 显示界面
     * */
    public void ShowPanel(string panelName, bool hideAll = true)
    {
        if (panels.ContainsKey(panelName))
        {
            if (hideAll)
            {
                HideAllPanel();
            }
            GameObject go = panels[panelName];
            go.SetActive(true);
        }
    }

    /**
     * 隐藏界面
     * */
    public void HidePanel(string panelName)
    {
        if (panels.ContainsKey(panelName))
        {
            GameObject go = panels[panelName];
            go.SetActive(false);
        }
    }

    /**
     * 隐藏所有界面(不包含主界面)
     * */
    public void HideAllPanel()
    {
        foreach (var item in panels)
        {
            item.Value.SetActive(false);
        }
    }

    /**
     * 获取GameObject
     * */
     public GameObject GetGameObjectByName(string panelName)
    {
        if (panels.ContainsKey(panelName))
        {
            return panels[panelName];
        }
        return null;
    }

    /**
     * 获取所有打开的界面
     * */
    private List<string> getOpenPanels()
    {
        return openPanels;
    }

    /**
     * 获取所有打开的界面
     * */
    public bool canDrawLine()
    {
        if (openPanels.Count == 1 && openPanels.Contains("PanelGame"))
        {
            return true;
        }
        if (openPanels.Count == 2 && openPanels.Contains("PanelGame") && openPanels.Contains("PanelPraise"))
        {
            return true;
        }
        return false;
    }

    /**
     * 显示log
     * */
    public void ShowLog(string text)
    {
        GameObject go = UIManager.Instance.GetGameObjectByName("PanelLog");
        Log log = go.GetComponent<Log>();
        if (log != null)
        {
            log.ShowLog(text);
            UIManager.Instance.ShowPanel("PanelLog", false);
        }
    }
}
