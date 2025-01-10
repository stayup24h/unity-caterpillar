using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettingController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;

    private string key = "Master";

    void Awake()
    {
        volumeSlider.onValueChanged.AddListener(setVolume);
    }

    void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat(key, 1);
        audioMixer.SetFloat(key, Mathf.Log10(volumeSlider.value) * 20);
    }

    public void setVolume(float volume)
    {
        audioMixer.SetFloat(key, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(key, volume);
        PlayerPrefs.Save();
    }
}
