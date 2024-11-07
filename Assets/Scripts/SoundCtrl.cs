using System.Collections;
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
        if(!isRunning_MoveSound) StartCoroutine(PlayMoveSound());
    }

    IEnumerator PlayMoveSound()
    {
        isRunning_MoveSound=true;
        while (true)
        {
            if (!moveSound.isPlaying)
            {
                moveSound.Play();
                yield return new WaitForSeconds(moveSound.clip.length + delayTime);
            }
            else
            {
                yield return null;
            }
        }
    }

    public void StartDefeatSound()
    {
        defeatSound.Play();
    }
}
