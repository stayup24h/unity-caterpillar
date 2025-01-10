using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance => instance;

    public List<AudioClip>[] moveSFXArr = new List<AudioClip>[3]; // 0->climb, 1->bubble, 2->jump
    [HideInInspector] public List<AudioClip> bgmLst = new List<AudioClip>();

    private int prevSFXType = -1;
    private int prevBgmType = -1;

    private AudioSource bgm;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        for (int i = 0; i < moveSFXArr.Length; i++)
        {
            moveSFXArr[i] = new List<AudioClip>();
        }

        foreach (AudioClip clip in Resources.LoadAll<AudioClip>("Audio/MoveSFX"))
        {
            if (clip.name.Contains("jump")) moveSFXArr[2].Add(clip);
            else if (clip.name.Contains("bubble")) moveSFXArr[1].Add(clip);
            else if (clip.name.Contains("climb")) moveSFXArr[0].Add(clip);
        }

        foreach (AudioClip clip in Resources.LoadAll<AudioClip>("Audio/BGM"))
        {
            bgmLst.Add(clip);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bgm = GetComponent<AudioSource>();
        bgm.enabled = true;
        bgm.playOnAwake = false;
        bgm.loop = true;
        ChangeBGMAndPlay();
        bgm.Play();

        ChangeSoundType();
    }

    public void ChangeSoundType()
    {
        while (true)
        {
            int soundType = Random.Range(0, 3); // 0->climb, 1->bubble, 2->jump
            if (soundType != prevSFXType)
            {
                prevSFXType = soundType;
                break;
            }
        }
    }

    public void ChangeBGMAndPlay()
    {
        while (true)
        {
            int soundType = Random.Range(0, bgmLst.Count);
            if (soundType != prevBgmType)
            {
                bgm.clip = bgmLst[soundType];
                prevBgmType = soundType;
                if (!bgm.isPlaying) bgm.Play();
                break;
            }
        }
    }

    public AudioClip GetMoveSFX()
    {
        List<AudioClip> clips = moveSFXArr[prevSFXType];
        return clips[Random.Range(0, clips.Count)];
    }
}
