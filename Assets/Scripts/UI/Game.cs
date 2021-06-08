using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {
    //跟随手指移动的对象（用于检测碰撞）
    public Transform panelWordMove;

    //是否碰撞
    public bool isCollision = true;

    //供选择的文字
    public GameObject[] PanelWords;
    public Text[] PanelWordTexts;

    //金币数量
    public Text TextGold;
    private int isPlayGoldChange = 0;
    private int curShowGold;
    private int willShowGold;

    //关卡
    public Text TextLevel;

    //正确结果文字
    [HideInInspector]
    public int resultIndexMax = 0;//最大正确个数
    private List<int> resultIndexHas = new List<int>();
    public GameObject[] PanelWordResults1;//文字父对象（包括背景）
    public GameObject[] PanelWordResults2;//文字内容
    public PanelWord[] PanelWord2;//文字内容
    public Text[] PanelWordTextResults;//显示的字
    private List<string> ResultTexts = new List<string>();

    //已经提示过的index
    private List<int> tipIndexs = new List<int>();

    //打开的界面
    private List<string> openPanels;

    //线
    public LineRenderer lineRenderer;
    public GameObject ImageLine;
    private int _segmentNum = 50;

    //点
    private List<Vector3> positions = new List<Vector3>();
    private List<int> indexs = new List<int>();
    private Vector3 pixel;

    //贝塞尔线
    private float distanceFrag = 0.3f;

    //是否画线
    private bool isDraw = false;

    //当前文字
    private List<string> CurTexts = new List<string>();
    public GameObject CurImageBg;
    public RectTransform CurImageBgRect;
    public GameObject[] CurWords;
    public Text[] CurWordTexts;

    //赞美index
    private int praiseIndex = 0;
    private int wrongIndex = 0;
    private List<int> goldRange = new List<int>() { 5, 10, 20, 30, 40 };

    //位置
    private List<Vector3> position6 = new List<Vector3>();
    private List<Vector3> position5 = new List<Vector3>();
    private List<Vector3> position4 = new List<Vector3>();
    private List<Vector3> positionFinal = new List<Vector3>();
    private int playRefresh = -1;
    private int playRefresh2 = -2;
    private Vector3 centerPosition = new Vector3(0, -427, 0);

    //当前关
    private int curGk = -1;

    //list脚本
    public ListView listView;

    // Use this for initialization
    void Start () {
        //CurTexts.Add("一");
        //CurTexts.Add("二");
        //CurTexts.Add("用");
        //CurTexts.Add("心");
        //CurTexts.Add("一");
        //CurTexts.Add("意");
        position6.Add(new Vector3(-161,-140,0));
        position6.Add(new Vector3(161,-140,0));
        position6.Add(new Vector3(312,-382,0));
        position6.Add(new Vector3(161,-630,0));
        position6.Add(new Vector3(-161,-630,0));
        position6.Add(new Vector3(-292,-382,0));

        position5.Add(new Vector3(0,-147,0));
        position5.Add(new Vector3(238,-361,0));
        position5.Add(new Vector3(159,-630,0));
        position5.Add(new Vector3(-161,-630,0));
        position5.Add(new Vector3(-226,-361,0));

        position4.Add(new Vector3(0,-147,0));
        position4.Add(new Vector3(269,-427,0));
        position4.Add(new Vector3(0,-684,0));
        position4.Add(new Vector3(-246,-427,0));

        positionFinal = position6;

        setGkShow(UserData.Instance.Gk + 1);
        TextGold.text = UserData.Instance.Gold.ToString();
        curShowGold = UserData.Instance.Gold;
        DataChange.Addlistener("gold", OnGoldChange);
    }

    private void OnGoldChange(object value)
    {
        if (curShowGold <= UserData.Instance.Gold)
        {
            isPlayGoldChange = 1;
        }
        else
        {
            isPlayGoldChange = -1;
        }
    }

    // Update is called once per frame
    void Update () {
        if (UIManager.Instance.canDrawLine() == true)
        {
            drawLine();
            if (Input.touchCount > 0)
            {
                //isDraw = true;
                //Touch touch = Input.GetTouch(0);
                //Vector3 screenPos = Camera.main.WorldToScreenPoint(panelWordMove.position); // 目的获取z，在Start方法
                //Vector3 mousePos = touch.position;
                //mousePos.z = screenPos.z; // 这个很关键
                //Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                //panelWordMove.position = worldPos;
            }
            else
            {
                //isDraw = false;
                //stopDrawLine();
            }
            if (Input.GetMouseButtonDown(0))
            {
                isDraw = true;
                
            }
            if (isDraw)
            {
                //transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                Vector3 screenPos = Camera.main.WorldToScreenPoint(panelWordMove.position); // 目的获取z，在Start方法
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = screenPos.z; // 这个很关键
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                panelWordMove.position = worldPos;
                //ImageLine.transform.position = worldPos;
            }
            if (Input.GetMouseButtonUp(0))
            {
                isDraw = false;
                stopDrawLine();
            }
        }

        if (playRefresh == 1)
        {
            for (int i = 0; i < CurTexts.Count; i++)
            {
                PanelWords[i].transform.localPosition = Vector3.Lerp(PanelWords[i].transform.localPosition, centerPosition, 0.2f);
                if (i == CurTexts.Count-1)
                {
                    if (Vector3.Distance(PanelWords[i].transform.localPosition, centerPosition) <= 0.1f)
                    {
                        playRefresh = 2;
                        Invoke("changePlayRefresh", 0.12f);
                        break;
                    }
                }
            }
        }
        else if (playRefresh == 3)
        {
            for (int i = 0; i < CurTexts.Count; i++)
            {
                PanelWords[i].transform.localPosition = Vector3.Lerp(PanelWords[i].transform.localPosition, positionFinal[i], 0.2f);
                if (i == CurTexts.Count - 1)
                {
                    if (Vector3.Distance(PanelWords[i].transform.localPosition, positionFinal[i]) <= 0.1f)
                    {
                        playRefresh = -1;
                        isCollision = true;
                        break;
                    }
                }
            }
        }
        if (playRefresh != playRefresh2)
        {
            if (playRefresh == 1) //开始移动
            {
                for (int i = 0; i < CurTexts.Count; i++)
                {
                    Animator animator = PanelWords[i].GetComponent<Animator>();
                    if (animator != null)
                    {
                        animator.SetBool("play", true);
                    }
                }
            }else if (playRefresh == -1) //停止移动
            {
                for (int i = 0; i < CurTexts.Count; i++)
                {
                    Animator animator = PanelWords[i].GetComponent<Animator>();
                    if (animator != null)
                    {
                        animator.SetBool("play", false);
                    }
                }
            }
            playRefresh2 = playRefresh;
        }


        if (isPlayGoldChange == 1)
        {
            willShowGold = Mathf.CeilToInt(Mathf.Lerp(curShowGold, UserData.Instance.Gold, 0.1f));
            if (willShowGold == curShowGold || curShowGold >= UserData.Instance.Gold || Mathf.Abs(curShowGold - UserData.Instance.Gold) <= 1)
            {
                curShowGold = UserData.Instance.Gold;
                TextGold.text = curShowGold.ToString();
                isPlayGoldChange = 0;
                return;
            }
            curShowGold = willShowGold;
            TextGold.text = curShowGold.ToString();
        }
        else if (isPlayGoldChange == -1)
        {
            willShowGold = Mathf.CeilToInt(Mathf.Lerp(curShowGold, UserData.Instance.Gold, 0.1f));
            if (willShowGold == curShowGold || curShowGold <= UserData.Instance.Gold || Mathf.Abs(curShowGold - UserData.Instance.Gold) <= 1)
            {
                curShowGold = UserData.Instance.Gold;
                TextGold.text = curShowGold.ToString();
                isPlayGoldChange = 0;
                return;
            }
            curShowGold = willShowGold;
            TextGold.text = curShowGold.ToString();
        }
    }
    private void changePlayRefresh()
    {
        playRefresh = 3;
        isCollision = false;
    }
    /**
     * 设置关卡显示
     * */
    public void setGkShow(int gk)
    {
        isCollision = true;
        curGk = Mathf.Min(gk, LoadConfig.Instance.data.Count);
        TextLevel.text = "关卡  "+curGk;
        JsonData config = LoadConfig.Instance.getGkConfig(curGk);
        if (config != null)
        {
            setResultTexts((string)config["resulttext"]);
            setCurTexts((string)config["curtext"]);
            refreshCurWord();
        }

        //重置赞美
        wrongIndex = 0;
        praiseIndex = 0;
    }
    /**
     * 设置结果文字
     * */
    public void setResultTexts(string value)
    {
        string[] arr = value.Split("|".ToCharArray());
        ResultTexts = new List<string>(arr);
        for (int i = 0; i < PanelWordTextResults.Length; i++)
        {
            PanelWordResults2[i].SetActive(false);
            PanelWord2[i].setColorAlpha(1f);
            if (i < ResultTexts.Count)
            {
                PanelWordTextResults[i].text = ResultTexts[i];
                PanelWordResults1[i].SetActive(true);
            }
            else
            {
                PanelWordTextResults[i].text = "";
                PanelWordResults1[i].SetActive(false);
            }
        }
        resultIndexMax = ResultTexts.Count / 4;
        resultIndexHas.Clear();
        tipIndexs.Clear();
    }

    /**
     * 设置文字
     * */
    public void setCurTexts(string value)
    {
        string[] arr = value.Split("|".ToCharArray());
        CurTexts = new List<string>(arr);
        for (int i = 0; i < PanelWordTexts.Length; i++)
        {
            if (i < CurTexts.Count)
            {
                PanelWordTexts[i].text = CurTexts[i];
                PanelWords[i].SetActive(true);
            }
            else
            {
                PanelWordTexts[i].text = "";
                PanelWords[i].SetActive(false);
            }
        }
        refreshPosition();
    }

    /**
     * 刷新位置
     * */
    private void refreshPosition()
    {
        positionFinal = new List<Vector3>();

        if (CurTexts.Count == 4)
        {
            List<int> indexTmp = new List<int>() { 0,1,2,3};
            System.Random random = new System.Random();
            
            for (int i = 0; i < 4; i++)
            {
                int randomIndex = random.Next(0, indexTmp.Count);
                int positionIndex = indexTmp[randomIndex];
                positionFinal.Add(position4[positionIndex]);
                indexTmp.RemoveAt(randomIndex);
            }
        }
        else if (CurTexts.Count == 5)
        {
            List<int> indexTmp = new List<int>() { 0, 1, 2, 3, 4 };
            System.Random random = new System.Random();

            for (int i = 0; i < 5; i++)
            {
                int randomIndex = random.Next(0, indexTmp.Count);
                int positionIndex = indexTmp[randomIndex];
                positionFinal.Add(position5[positionIndex]);
                indexTmp.RemoveAt(randomIndex);
            }
        }
        else if(CurTexts.Count == 6)
        {
            List<int> indexTmp = new List<int>() { 0, 1, 2, 3, 4, 5 };
            System.Random random = new System.Random();

            for (int i = 0; i < 6; i++)
            {
                int randomIndex = random.Next(0, indexTmp.Count);
                int positionIndex = indexTmp[randomIndex];
                positionFinal.Add(position6[positionIndex]);
                indexTmp.RemoveAt(randomIndex);
            }
        }
        else
        {
            Debug.Log("Error:CurTexts.lenght is wrong!");
        }
        playRefresh = 1;
        isCollision = false;
    }

    /**
     * 刷新当前文字的显示
     * */
    private void refreshCurWord()
    {
        if (indexs.Count >= 1)
        {
            CurImageBgRect.sizeDelta = new Vector2(40+110*indexs.Count, CurImageBgRect.rect.height);
            CurImageBg.SetActive(true);
            for (int i = 0; i < indexs.Count; i++)
            {
                if (i < CurWordTexts.Length)
                {
                    CurWordTexts[i].text = CurTexts[indexs[i]];
                }
            }
        }
        else
        {
            CurImageBg.SetActive(false);
        }
        for (int i = 0; i < CurWords.Length; i++)
        {
            if (i < indexs.Count)
            {
                CurWords[i].SetActive(true);
            }
            else
            {
                CurWords[i].SetActive(false);
            }
        }
    }

    /**
     * 画线
     * */
    private void drawLine()
    {
        //Debug.Log("positions.Count:"+ positions.Count);
        if (positions.Count >= 1)
        {
            if (positions.Count == 1)
            {
                lineRenderer.positionCount = _segmentNum;
                for (int j = 0; j < _segmentNum; j++)
                {
                    float t = j / (float)_segmentNum;
                    pixel = positions[0] + (panelWordMove.position - positions[0]) * t;
                    pixel += new Vector3(0, 0, -0.1f);
                    lineRenderer.SetPosition(j, pixel);
                }
            }
            else
            {
                lineRenderer.positionCount = _segmentNum * positions.Count;
                List<Vector3> p = new List<Vector3>();
                for (int i = 0; i < positions.Count; i++)
                {
                    p.Add(positions[i]);
                }
                p.Add(panelWordMove.position);
                for (int i = 0; i < p.Count-1; i++)
                {
                    for (int j = 0; j < _segmentNum; j++)
                    {
                        float t = j / (float)(_segmentNum);
                        pixel = Bezier(p[i] + GetHandle(p,i,false), p[i], p[i+1], p[i+1] + GetHandle(p, i+1, true), t);
                        pixel += new Vector3(0, 0, -0.1f);
                        lineRenderer.SetPosition(j + _segmentNum*i, pixel);
                    }
                }
            }
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }
    
    private List<Vector3> getNewP(List<Vector3> p)
    {
        List<Vector3> pBezier = new List<Vector3>();
        List<Vector3> pTmp = new List<Vector3>();
        for (int i = 0; i < p.Count; i++)
        {
            pBezier.Add(p[i]);
            pTmp.Add(p[i]);
        }
        int n = pTmp.Count;
        for (int i = 1; i < pTmp.Count - 1; i++)
        {
            Vector3 p0 = pTmp[0];
            Vector3 p1 = pTmp[i];
            Vector3 p2 = pTmp[i + 1];
            pBezier[i] = (p1-(float)i/(n - 1)*(p2+p0))*(n-1)/(float)i + (float)i/(n-1)*(p2+p0);
        }
        return pBezier;
    }
    private Vector3 getBezierCurve( List<Vector3> p, float t)
    {
        if (p.Count < 2)
        {
            return p[0];
        }
        List<Vector3> newp = new List<Vector3>();
        for (int i = 0; i < p.Count - 1; i++)
        {
            Vector3 p0p1 = (1 - t) * p[i] + t * p[i + 1];
            newp.Add(p0p1);
        }
        return getBezierCurve(newp, t);
    }

    public Vector3 GetHandle(List<Vector3> p, int index, bool isin)
    {
        object p0 = null;
        object p1 = p[index];
        object p2 = null;
        if (index - 1 >= 0)
        {
            p0 = p[index - 1];
        }
        if (index + 1 <= p.Count - 1)
        {
            p2 = p[index + 1];
        }
        if (isin)
        {
            return GetHandleIn(p0, p1, p2);
        }
        return GetHandleOut(p0, p1, p2);
    }

    public Vector3 GetHandleIn(object p0, object p1, object p2)
    {
        Vector3 p;
        if (p0 != null)
        {
            p = (Vector3)p0 - (Vector3)p1;
        }
        else
        {
            p = (Vector3)p1 - (Vector3)p2;
        }
        Vector3 n;
        if (p2 != null)
        {
            n = (Vector3)p2 - (Vector3)p1;
        }
        else
        {
            n = (Vector3)p1 - (Vector3)p0;
        }
        return GetHandleIn(p, n);
    }
    public Vector3 GetHandleIn(Vector3 p, Vector3 n)
    {
        float pLen = p.magnitude;
        float nLen = n.magnitude;
        Vector3 pIn = Vector3.zero;
        Vector3 pOut = Vector3.zero;

        if (pLen != 0 || nLen != 0)
        {
            Vector3 dir = ((pLen / nLen) * n - p).normalized;
            pIn = -dir * (pLen * distanceFrag);
            pOut = dir * (nLen * distanceFrag);
        }
        return pIn;
    }

    public Vector3 GetHandleOut(object p0, object p1, object p2)
    {
        Vector3 p;
        if (p0 != null)
        {
            p = (Vector3)p0 - (Vector3)p1;
        }
        else
        {
            p = (Vector3)p1 - (Vector3)p2;
        }
        Vector3 n;
        if (p2 != null)
        {
            n = (Vector3)p2 - (Vector3)p1;
        }
        else
        {
            n = (Vector3)p1 - (Vector3)p0;
        }
        return GetHandleOut(p, n);
    }
    public Vector3 GetHandleOut(Vector3 p, Vector3 n)
    {
        float pLen = p.magnitude;
        float nLen = n.magnitude;
        Vector3 pIn = Vector3.zero;
        Vector3 pOut = Vector3.zero;

        if (pLen != 0 || nLen != 0)
        {
            Vector3 dir = ((pLen / nLen) * n - p).normalized;
            pIn = -dir * (pLen * distanceFrag);
            pOut = dir * (nLen * distanceFrag);
        }
        return pOut;
    }

    //T0=P0+P0.HandleOut
    //T1=P1+P1.HandleIn
    private Vector3 Bezier(Vector3 T0, Vector3 P0, Vector3 P1, Vector3 T1, float f)
    {
        double Ft2 = 3; double Ft3 = -3;
        double Fu1 = 3; double Fu2 = -6; double Fu3 = 3;
        double Fv1 = -3; double Fv2 = 3;

        double FAX = -P0.x + Ft2 * T0.x + Ft3 * T1.x + P1.x;
        double FBX = Fu1 * P0.x + Fu2 * T0.x + Fu3 * T1.x;
        double FCX = Fv1 * P0.x + Fv2 * T0.x;
        double FDX = P0.x;

        double FAY = -P0.y + Ft2 * T0.y + Ft3 * T1.y + P1.y;
        double FBY = Fu1 * P0.y + Fu2 * T0.y + Fu3 * T1.y;
        double FCY = Fv1 * P0.y + Fv2 * T0.y;
        double FDY = P0.y;

        double FAZ = -P0.z + Ft2 * T0.z + Ft3 * T1.z + P1.z;
        double FBZ = Fu1 * P0.z + Fu2 * T0.z + Fu3 * T1.z;
        double FCZ = Fv1 * P0.z + Fv2 * T0.z;
        double FDZ = P0.z;

        float FX = (float)(((FAX * f + FBX) * f + FCX) * f + FDX);
        float FY = (float)(((FAY * f + FBY) * f + FCY) * f + FDY);
        float FZ = (float)(((FAZ * f + FBZ) * f + FCZ) * f + FDZ);

        return new Vector3(FX, FY, FZ);
    }

    /**
     * 结束画线
     * */
    private void stopDrawLine()
    {
        if (indexs.Count > 0)
        {
            //是否显示赞美
            bool showPraise = false;

            //检测是否正确
            int startIndex = checkResult();
            if (startIndex >= 0)
            {
                for (int i = startIndex*4; i < startIndex*4+4; i++)
                {
                    if (i < PanelWordResults2.Length)
                    {
                        PanelWordResults2[i].SetActive(true);
                        PanelWord2[i].setColorAlpha(1f);
                    }
                }

                //处理赞美的显示
                //Debug.Log(wrongIndex + "," + praiseIndex + "," + resultIndexMax);
                if (wrongIndex + praiseIndex < resultIndexMax)
                {
                    showPraise = true;
                    praiseIndex++;
                    AudioManager.Instance.PlayEffect("praise" + praiseIndex);
                    UIManager.Instance.SwitchPanel("PanelPraise", false, false);
                    GameObject go = UIManager.Instance.GetGameObjectByName("PanelPraise");
                    Praise praise = go.GetComponent<Praise>();
                    if (praise != null)
                    {
                        praise.ShowIndex(praiseIndex);
                    }
                }
            }
            else if (startIndex == -4) //长度无效
            {
                Debug.Log("长度无效");
            }
            else if (startIndex == -1) //长度不满足条件
            {
                Debug.Log("长度不满足条件");
            }
            else if (startIndex == -2) //已经找到过
            {
                Debug.Log("已经找到过");
            }
            else if (startIndex == -3) //未匹配上正确的成语
            {
                Debug.Log("未匹配上正确的成语");
            }
            else if (startIndex == -10) //已找到所有成语
            {
                Debug.Log("已找到所有成语");
                //清除数据
                positions.Clear();
                indexs.Clear();
                refreshCurWord();
                drawLine();
                return;
            }

            //清除数据
            positions.Clear();
            indexs.Clear();
            refreshCurWord();

            //找到所有成语
            if (resultIndexHas.Count == resultIndexMax) // 已找到所有成语
            {
                drawLine(); //清除残留的线
                isCollision = false;
                if (showPraise) //有赞美延迟1.5秒显示
                {
                    Invoke("delayShowWin", 1.5f);
                }
                else 
                {
                    //没有赞美马上显示
                    Invoke("delayShowWin", 1.1f);
                }
            }

            //记录错误次数
            if (startIndex < 0 && startIndex != -4) // 错误但长度不小于1（有效错误）
            {
                wrongIndex++;
            }
        }
    }
    private void delayShowWin()
    {
        AudioManager.Instance.PlayEffect("clear");
        UIManager.Instance.SwitchPanel("PanelWin", false, false);
    }
    private int checkResult()
    {
        if (resultIndexHas.Count == resultIndexMax) // 已找到所有成语
        {
            return -10;
        }
        if (indexs.Count <= 1) // 长度无效
        {
            return -4;
        }
        if (indexs.Count != 4) // 长度不满足
        {
            return -1;
        }
        int indexTmp = -3;
        for (int i = 0; i < resultIndexMax; i++)
        {
            bool right = true;
            for (int j = 0; j < 4; j++)
            {
                if (ResultTexts[j + i*4] != CurTexts[indexs[j]])
                {
                    right = false;
                    break;
                }
            }
            if (right) // 找到一个符合条件的
            {
                indexTmp = i;
                break;
            }
        }
        if (indexTmp != -3)
        {
            if (resultIndexHas.IndexOf(indexTmp) != -1) //重复
            {
                indexTmp = -2;
            }
            else
            {
                resultIndexHas.Add(indexTmp);
            }
        }
        return indexTmp;
    }
    private void showTip()
    {
        int noFindIndex = getNoFindIndex();
        if (noFindIndex > -1)
        {
            if (noFindIndex < PanelWord2.Length)
            {
                PanelWordResults2[noFindIndex].SetActive(true);
                PanelWord2[noFindIndex].setColorAlpha(0.3f);
                tipIndexs.Add(noFindIndex);
            }
        }
        else
        {
            Debug.Log("找到所有成语，不需要提示!");
        }
    }
    private int getNoFindIndex()
    {
        List<int> noFindIndexs = new List<int>();
        for (int i = 0; i < resultIndexMax; i++)
        {
            if (resultIndexHas.Contains(i) == false)//未找到的组
            {
                for (int j = 0; j < 4; j++)
                {
                    int noFindIndex = i * 4 + j;//未找到的编号
                    if (tipIndexs.Contains(noFindIndex) == false) //未提示过的编号
                    {
                        noFindIndexs.Add(noFindIndex);
                    }
                }
            }
        }
        if (noFindIndexs.Count > 0)
        {
            System.Random random = new System.Random();
            int randomIndex = random.Next(0, noFindIndexs.Count);
            return noFindIndexs[randomIndex];
        }
        return -1;
    }

    /**
     * 手指碰到文字
     * */
    public void enterIndex(Transform enterTransform, int index)
    {
        if (isCollision == false)
        {
            return;
        }
        if(indexs.IndexOf(index) == -1) //没有在列表里
        {
            positions.Add(enterTransform.position);
            //Debug.Log("addposition:("+ enterTransform.position.ToString() + ")");
            indexs.Add(index);
            refreshCurWord();
        }
        else
        {
            if (indexs.IndexOf(index) == indexs.Count - 2) // 在列表的倒数第2位
            {
                positions.RemoveAt(positions.Count - 1); // 移除掉最后一位
                indexs.RemoveAt(indexs.Count - 1); // 移除掉最后一位
                refreshCurWord();
            }
        }
    }

    /**
     * 手指离开文字
     * */
    public void exitIndex(Transform exitTransform, int index)
    {

    }

    /**
     * 点击商店按钮
     * */
    public void ClickPanelGold()
    {
#if UNITY_ANDROID

#else
        AudioManager.Instance.PlayEffect("click");
        UIManager.Instance.SwitchPanel("PanelShop",false,false);
#endif
    }

    /**
     * 点击关卡按钮
     * */
    public void ClickPaneLevel()
    {
        AudioManager.Instance.PlayEffect("click");
        UIManager.Instance.SwitchPanel("PanelLevel");
        listView.ScrollToIndex(UserData.Instance.Gk);
    }

    /**
     * 点击求助按钮
     * */
    public void ClickButtonTip()
    {
        AudioManager.Instance.PlayEffect("click");
        if (UserData.Instance.Gold >= 120)
        {
            int noFindIndex = getNoFindIndex();
            if (noFindIndex > -1)
            {
                UserData.Instance.addGold(-120);
                showTip();
            }
        }
        //Debug.Log("click tip");
    }

    /**
     * 点击刷新按钮
     * */
    public void ClickButtonRefresh()
    {
        AudioManager.Instance.PlayEffect("click");
        if (playRefresh == -1)
        {
            refreshPosition();
        }
        //Debug.Log("click refresh");
    }

    /**
     * 点击胜利界面背景
     * */
    public void ClickWinBg()
    {
        UIManager.Instance.HidePanel("PanelWin");
        if (curGk > UserData.Instance.Gk)
        {
            UserData.Instance.setGk(curGk);
            System.Random random = new System.Random();
            int praiseIndexTmp = Mathf.Clamp(praiseIndex, 0, 3);
            int addgold = random.Next(goldRange[praiseIndexTmp], goldRange[praiseIndexTmp+1]);
            //Debug.Log("addgold:" + addgold.ToString());
            UserData.Instance.addGold(addgold);
        }
        System.Random random2 = new System.Random();
        int randomNum = random2.Next(0, 100);
        int rate = 10 + 10 * curGk / LoadConfig.Instance.data.Count;
        //Debug.Log("randomNum:" + randomNum.ToString() + "    rate:" + rate.ToString());
        //if (curGk >= 10 && randomNum < rate)
        if (false)
        {
            Global.curgk = curGk;
            GoogleAdmob.Instance.ShowAd();
        }
        else
        {
            setGkShow(curGk + 1);
        }
    }
}
