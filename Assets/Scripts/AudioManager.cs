using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    private static AudioManager instance;
    //背景音乐
    public AudioSource audioBg;
    public AudioSource audioEffect;
    public AudioClip clipClick;
    public AudioClip clear;
    public AudioClip praise1;
    public AudioClip praise2;
    public AudioClip praise3;

    public static AudioManager Instance
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

    void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start () {
        if (Global.IsPlayMusic())
        {
            audioBg.Play();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /**
     * 播放背景音乐
     * */
    public void PlayMusic(bool value)
    {
        if (value)
        {
            audioBg.Play();
        }
        else
        {
            audioBg.Stop();
        }
    }

    /**
     * 播放音效
     * */
    public void PlayEffect(string effectName)
    {
        if (Global.IsPlaySoundEffect() == false)
        {
            return;
        }
        switch (effectName)
        {
            case "click":
                audioEffect.clip = clipClick;
                break;

            case "clear":
                audioEffect.clip = clear;
                break;

            case "praise1":
                audioEffect.clip = praise1;
                break;

            case "praise2":
                audioEffect.clip = praise2;
                break;

            case "praise3":
                audioEffect.clip = praise3;
                break;

            default:
                break;
        }
        
        audioEffect.Play();
    }
}
