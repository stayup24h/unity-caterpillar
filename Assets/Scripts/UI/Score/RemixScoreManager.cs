using UnityEngine;
using TMPro;
using eventChannel;

public class RemixScoreManager : ScoreManager
{
    [SerializeField] private Transform mapPool;
    [SerializeField] private Transform caterpillarHead;

    [Header("EndPanel")]
    [SerializeField] private GameObject endPanel;
    [SerializeField] private TextMeshProUGUI endScoreText;
    [SerializeField] private TextMeshProUGUI endBestScoreText;

    [Header("EventChannel")]
    [SerializeField] private EventChannelSO endEventChannel;

    private Transform prevScorePoint;

    private void OnEnable()
    {
        endEventChannel.OnEventRaised += UpdateBestScore;
        endEventChannel.OnEventRaised += SetEndScore;
    }

    private void OnDisable()
    {
        endEventChannel.OnEventRaised -= UpdateBestScore;
        endEventChannel.OnEventRaised -= SetEndScore;
    }

    void Start()
    {
        if (GameManager.Instance.MapType == MapType.stage)
        {
            enabled = false;
            return;
        }

        key = "Remix";
        if (caterpillarHead == null) caterpillarHead = GameObject.Find("Caterpillar").transform.GetChild(0);
        score = 0;
        SetScore();
    }

    void Update()
    {
        Transform currentScorePoint = mapPool.GetChild(1).Find("Start Point");
        if (prevScorePoint != currentScorePoint)
        {
            if (caterpillarHead.position.x > currentScorePoint.position.x)
            {
                score++;
                SetScore();
                SoundManager.Instance.ChangeSoundType();
                prevScorePoint = currentScorePoint;
            }
        }
    }

    private void SetScoreToText(TextMeshProUGUI text, int score)
    {
        text.text = ((score / 100 == 0) ? "0" : "") + ((score / 10 == 0) ? "0" : "") + score.ToString();
    }

    private void SetScore()
    {
        int showScore = score % 1000;
        SetScoreToText(scoreText, showScore);
    }

    private void SetEndScore()
    {
        SetScoreToText(endScoreText, score);
        SetScoreToText(endBestScoreText, PlayerPrefs.GetInt(key, 0));
        endPanel.SetActive(true);
    }
}
