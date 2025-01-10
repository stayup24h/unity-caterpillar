using eventChannel;
using UnityEngine;

public class StageScoreManager : ScoreManager
{
    [SerializeField] private GameObject failPanel;
    [SerializeField] private GameObject clearPanel;

    [Header("EventChannel")]
    [SerializeField] private EventChannelSO clearEventChannel;
    [SerializeField] private EventChannelSO endEventChannel;

    private void OnEnable()
    {
        clearEventChannel.OnEventRaised += UpdateBestScore;
        clearEventChannel.OnEventRaised += OpenClearPanel;
        endEventChannel.OnEventRaised += OpenFailPanel;
    }

    private void OnDisable()
    {
        clearEventChannel.OnEventRaised -= UpdateBestScore;
        clearEventChannel.OnEventRaised -= OpenClearPanel;
        endEventChannel.OnEventRaised -= OpenFailPanel;
    }

    void Start()
    {
        if (GameManager.Instance.MapType == MapType.remix)
        {
            enabled = false;
            return;
        }

        key = "Stage";
        score = GameManager.Instance.StageNum;
        scoreText.text = "#" + ((score / 10 == 0) ? "0" : "") + score.ToString();
    }

    private void OpenClearPanel()
    {
        clearPanel.SetActive(true);
    }

    private void OpenFailPanel()
    {
        failPanel.SetActive(true);
    }
}