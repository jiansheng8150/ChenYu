using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using admob;

public class GoogleAdmob : MonoBehaviour
{
    private static GoogleAdmob instance;
    public static GoogleAdmob Instance
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

    private bool testMode = false;

    Admob ad;
    string bannerID = "";
    string interstitialID = "";
    string videoID = "";
    string nativeBannerID = "";

    private int count = 0;

    void Awake()
    {
        instance = this;

        initAdmob();
    }

    void initAdmob()
    {
#if UNITY_IOS
				 bannerID="ca-app-pub-3940256099942544/2934735716";
				 interstitialID="ca-app-pub-3940256099942544/4411468910";
				 videoID="ca-app-pub-3940256099942544/1712485313";
				 nativeBannerID = "ca-app-pub-3940256099942544/3986624511";
#elif UNITY_ANDROID
        bannerID = "ca-app-pub-3940256099942544/6300978111";
        interstitialID = "ca-app-pub-3940256099942544/1033173712";
        videoID = "ca-app-pub-1218830301252607/8950230704";
        //videoID = "ca-app-pub-3940256099942544/5224354917";
        nativeBannerID = "ca-app-pub-3940256099942544/2247696110";
#endif
        AdProperties adProperties = new AdProperties();
        adProperties.isTesting(testMode);
        adProperties.isAppMuted(false);
        adProperties.isUnderAgeOfConsent(false);
        adProperties.appVolume(100);
        adProperties.maxAdContentRating(AdProperties.maxAdContentRating_G);
        string[] keywords = { "diagram", "league", "brambling" };
        adProperties.keyworks(keywords);

        ad = Admob.Instance();
        ad.bannerEventHandler += onBannerEvent;
        ad.interstitialEventHandler += onInterstitialEvent;
        ad.rewardedVideoEventHandler += onRewardedVideoEvent;
        ad.nativeBannerEventHandler += onNativeBannerEvent;
        ad.initSDK(adProperties);//reqired,adProperties can been null
    }

    void Start()
    {
        ad.loadRewardedVideo(videoID);
    }
    
    void onInterstitialEvent(string eventName, string msg)
    {
        Debug.Log("handler onAdmobEvent---" + eventName + "   " + msg);
        if (eventName == AdmobEvent.onAdLoaded)
        {
            Admob.Instance().showInterstitial();
        }
    }

    void onBannerEvent(string eventName, string msg)
    {
        Debug.Log("handler onAdmobBannerEvent---" + eventName + "   " + msg);
    }

    void onRewardedVideoEvent(string eventName, string msg)
    {
        string returnMsg = "handler onRewardedVideoEvent---" + eventName + "  rewarded: " + msg;
        Debug.Log(returnMsg);
        //UIManager.Instance.ShowLog(returnMsg);
        if (eventName == AdmobEvent.onAdClosed)
        {
            UIManager.Instance.HidePanel("PanelWaitingImage");
            ad.loadRewardedVideo(videoID);

            GameObject go = UIManager.Instance.GetGameObjectByName("PanelGame");
            if (go != null)
            {
                Game game = go.GetComponent<Game>();
                if (game != null)
                {
                    game.setGkShow(Global.curgk);
                }
            }
        }else if (eventName == AdmobEvent.onAdFailedToLoad)
        {
            if (count == 0)
            {
                UIManager.Instance.ShowLog("广告加载失败，请稍后再试！");
                UIManager.Instance.HidePanel("PanelWaitingImage");
                ad.loadRewardedVideo(videoID);
                count = count + 1;
            }
        }
    }

    void onNativeBannerEvent(string eventName, string msg)
    {
        Debug.Log("handler onAdmobNativeBannerEvent---" + eventName + "   " + msg);
    }

    public void ShowAd()
    {
        UIManager.Instance.ShowPanel("PanelWaitingImage", false);
        StartCoroutine(ShowAdWhenReady());
    }

    IEnumerator ShowAdWhenReady()
    {
        while (!ad.isRewardedVideoReady())
        {
            yield return null;
        }
        ad.showRewardedVideo();
    }
}
