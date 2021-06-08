using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public enum SCROLLTYPE
{
    Sigle = 1,
    Multi = 2
}

public class PageView: MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    //滚动条
    public Scrollbar m_Scrollbar;

    //滚动类型
    public SCROLLTYPE ScrollType;

    //需要滚动的容器
    public GameObject Content;

    //子对象宽高
    public Vector2 ChildSize;

    //子对象个数
    private int childCount;

    //滚动方向
    private UnityEngine.UI.Scrollbar.Direction scrollDirection;

    //位置阀值（超过阀值，跳到下一页）
    private Vector2 overPos;

    //按下时Scroll Index
    private int indexDown;

    //滚动条目标位置
    private int index;
    private float mTargetValue;

    //是否要移动
    private bool mNeedMove = false;

    //多项滚动，移动参数
    private const float SMOOTH_TIME_MULTI = 0.08f;
    private const float END_SCROLL_VALUE_MULTI = 0.0025f;

    //单项滚动，移动参数
    private const float SMOOTH_TIME_SIGLE = 0.18f;
    private const float END_SCROLL_VALUE_SIGLE = 0.004f;

    //每次移动的距离（百分比）
    private float smooth_time;

    //结束移动的条件（滚动条value变化）
    private float end_scroll_value;

    //当前播放声音的Scroll Index(用于播放声音)
    private bool isPlaySound = true;
    private int curIndex;
    private AudioSource tickSound;


    void Start()
    {
        tickSound = GetComponent(typeof(AudioSource)) as AudioSource; 
        childCount = Content.transform.childCount;
        overPos = new Vector2((float)3 / Screen.width, (float)1 / Screen.height);
        scrollDirection = m_Scrollbar.direction;
    }

    /**
     * 滚动value改变
     * */
    public void OnValueChanged()
    {
        if (isPlaySound && tickSound != null)
        {
            bool isPassOneIndex = passOneIndex(curIndex, m_Scrollbar.value);
            if (isPassOneIndex)
            {
                curIndex = getClosestIndex(m_Scrollbar.value);
                tickSound.Play();
            }
        }
    }

    /**
     * 经过一项
     * */
    private bool passOneIndex(int curIndex, float sValue)
    {
        float curValueMin = (float)(curIndex-1) / (childCount - 1);
        float curValueMax = (float)(curIndex+1) / (childCount - 1);
        if (sValue > curValueMin && sValue < curValueMax)
        {
            return false;
        }
        return true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        mNeedMove = false;
        isPlaySound = true;
        childCount = Content.transform.childCount;
        indexDown = getClosestIndex(m_Scrollbar.value);
        curIndex = getClosestIndex(m_Scrollbar.value);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (childCount <= 0)
        {
            return;
        }
        if (ScrollType == SCROLLTYPE.Sigle)
        {
            //Debug.Log("eventData.delta:" + eventData.delta);
            index = getClosestIndex(m_Scrollbar.value);
            if (index == indexDown)
            {
                if (scrollDirection == UnityEngine.UI.Scrollbar.Direction.LeftToRight || scrollDirection == UnityEngine.UI.Scrollbar.Direction.RightToLeft)
                {
                    if (Mathf.Abs(eventData.delta.x)/Screen.width > overPos.x)
                    {
                        if (eventData.delta.x > 0)
                        {
                            index -= 1;
                            index = Mathf.Max(0, index);
                        }
                        else
                        {
                            index += 1;
                            index = Mathf.Min(childCount-1, index);
                        }
                    }
                }
                else
                {
                    if (Mathf.Abs(eventData.delta.y) / Screen.height > overPos.y)
                    {
                        if (eventData.delta.y > 0)
                        {
                            index -= 1;
                            index = Mathf.Max(0, index);
                        }
                        else
                        {
                            index += 1;
                            index = Mathf.Min(childCount - 1, index);
                        }
                    }
                }
            }
            else if (index > indexDown)
            {
                index = indexDown + 1;
            }
            else if (index < indexDown)
            {
                index = indexDown - 1;
            }
            smooth_time = SMOOTH_TIME_SIGLE;
            end_scroll_value = END_SCROLL_VALUE_SIGLE;
            float closestValue = (float)index / (childCount - 1);
            mTargetValue = closestValue;
            mNeedMove = true;
        }
        else if (ScrollType == SCROLLTYPE.Multi)
        {
            index = getClosestIndex(m_Scrollbar.value);
            int addIndex;
            if (scrollDirection == UnityEngine.UI.Scrollbar.Direction.LeftToRight || scrollDirection == UnityEngine.UI.Scrollbar.Direction.RightToLeft)
            {
                addIndex = (int)(eventData.delta.x / Time.deltaTime / ChildSize.x);
                if (Mathf.Abs(eventData.delta.x) < 3)
                {
                    addIndex = 0;
                }
            }
            else
            {
                if (Mathf.Abs(eventData.delta.y) < 3)
                {
                    addIndex = 0;
                }
                addIndex = (int)(eventData.delta.y / Time.deltaTime / ChildSize.y);

            }
            addIndex = Mathf.RoundToInt(addIndex / 2);
            //Debug.Log("addIndex:" + addIndex + "    y:" + eventData.delta.y + "    x:" + eventData.delta.x);
            index -= addIndex;
            index = Mathf.Max(0, index);
            index = Mathf.Min(childCount - 1, index);

            smooth_time = SMOOTH_TIME_MULTI;
            end_scroll_value = END_SCROLL_VALUE_MULTI;
            float closestValue = (float)index / (childCount - 1);
            mTargetValue = closestValue;
            mNeedMove = true;
        }
    }

    /**
     * 获取最近的Scroll Index
     * */
    private int getClosestIndex(float sValue)
    {
        int indexTmp = Mathf.RoundToInt(sValue * (childCount - 1));
        return indexTmp;
        
    }

    public void OnButtonClick(int value)
    {
        switch (value)
        {
            case 1:
                mTargetValue = 0;
                break;
            case 2:
                mTargetValue = 0.25f;
                break;
            case 3:
                mTargetValue = 0.5f;
                break;
            case 4:
                mTargetValue = 0.75f;
                break;
            case 5:
                mTargetValue = 1f;
                break;
            default:
                Debug.LogError("!!!!!");
                break;
        }
        mNeedMove = true;
    }

    void Update()
    {
        if (mNeedMove)
        {
            if (Mathf.Abs(m_Scrollbar.value - mTargetValue) < end_scroll_value)
            {
                m_Scrollbar.value = mTargetValue;
                mNeedMove = false;
                return;
            }
            m_Scrollbar.value = Mathf.Lerp(m_Scrollbar.value, mTargetValue, smooth_time);
        }
    }

    /**
     * 设置当前位置(编号)
     * */
    public void SetIndex(int value)
    {
        isPlaySound = false;
        if (childCount <= 0)
        {
            m_Scrollbar.value = 1;
            index = childCount - 1;
        }
        else
        {
            m_Scrollbar.value = (float)(childCount - 1 - value) / (childCount - 1);
            index = childCount - 1 - value;
        }
    }

    /**
     * 获得当前位置(编号)
     * */
    public int GetIndex()
    {
        return (childCount - 1) - index;
    }
}