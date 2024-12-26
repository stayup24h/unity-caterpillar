using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get => instance; }

    public bool isHeadTurn { get; private set; }

    public List<AudioClip>[] moveSFXArr = new List<AudioClip>[3]; // 0->climb, 1->bubble, 2->jump
    public List<AudioClip> bgmLst = new List<AudioClip>();

    private int prevSFXType = -1;
    private int prevBgmType = -1;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        isHeadTurn = true;

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

    public void ChangeAndGetBGM()
    {
        while (true)
        {
            int soundType = Random.Range(0, bgmLst.Count);
            if (soundType != prevBgmType)
            {
                prevBgmType = soundType;
                break;
            }
        }
    }

    public AudioClip GetMoveSFX()
    {
        ChangeSoundType();//test¿ë
        List<AudioClip> clips = moveSFXArr[prevSFXType];
        return clips[Random.Range(0, clips.Count)];
    }
    

    [ContextMenu("CCCCC")]
    public void Change()
    {
        isHeadTurn = !isHeadTurn;
        print("ÅÏ Change " + isHeadTurn);
    }
}
