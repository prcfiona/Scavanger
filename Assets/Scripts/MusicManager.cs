using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    private static MusicManager _instance;

    public static MusicManager Instance
    {
        get { return _instance; }
    }

    public float miniPitch = 0.9f;
    public float maxPitch = 1.1f;

    public AudioSource efxSource;
    public AudioSource bgSource;

    void Awake()
    {
        _instance = this;
    }

    //随机播放音乐
	public void RandomPlay(params AudioClip[] clips)
    {
        float pitch = Random.Range(miniPitch, maxPitch);
        int index = Random.Range(0, clips.Length);
        AudioClip clip = clips[index];
        efxSource.clip = clip;
        efxSource.pitch = pitch;
        efxSource.Play();
    }
    public void StopBGMusic()
    {
        bgSource.Stop();
    }
}
