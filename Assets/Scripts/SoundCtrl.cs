using System.Collections.Generic;
using UnityEngine;

public class SoundCtrl : MonoBehaviour
{
    public AudioSource moveSound;
    public AudioSource defeatSound;
    public AudioSource clearSound;

    public float delayTime;
    public bool isRunning_MoveSound;
    private void Awake()
    {
        isRunning_MoveSound = false;
    }

    public void StartMoveSound()
    {
        moveSound.clip = GameManager.Instance.GetMoveSFX();
        moveSound.Play();
    }

    public void StartDefeatSound()
    {
        defeatSound.Play();
    }

    public void StartClearSound()
    {
        clearSound.Play();
    }
}
