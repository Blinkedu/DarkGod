using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 声音服务
/// </summary>
public class AudioSvc : MonoBehaviour
{
    public static AudioSvc Instance = null;

    public AudioSource bgAudio;
    public AudioSource uiAudio;

    public void InitSvc()
    {
        Instance = this;
        PECommon.Log("Init AudioSvc...");
    }

    public void PlayBGMusic(string name,bool isLoop = true)
    {
        AudioClip clip = ResSvc.Instance.LoadAudio("ResAudio/" + name, true);
        if(bgAudio.clip ==null || bgAudio.clip.name != clip.name)
        {
            bgAudio.clip = clip;
            bgAudio.loop = isLoop;
            bgAudio.Play();
        }
    }

    public void PlayUIAudio(string name)
    {
        AudioClip clip = ResSvc.Instance.LoadAudio("ResAudio/" + name, true);
        uiAudio.clip = clip;
        uiAudio.Play();
    }

    public void PlayCharAudio(string name,AudioSource source)
    {
        AudioClip clip = ResSvc.Instance.LoadAudio("ResAudio/" + name, true);
        source.clip = clip;
        source.Play();
    }

    public void StopBGMusic()
    {
        if (bgAudio != null)
        {
            bgAudio.Stop();
        }
    }
}
