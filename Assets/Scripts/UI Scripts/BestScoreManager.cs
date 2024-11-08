using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BestScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stageBestScore;
    [SerializeField] private TextMeshProUGUI remixBestScore;
    [SerializeField] private TextMeshProUGUI scoreText;

    void Awake()
    {
        if (stageBestScore != null && remixBestScore != null)
        {
            ShowBestScore();
        }
        //ResetScore();
    }

    private void ShowBestScore()
    {
        int score = PlayerPrefs.GetInt("Stage", 0);
        stageBestScore.text = "BEST\n#" + ((score / 10 == 0) ? "0" : "") + score.ToString();

        remixBestScore.text = "BEST\n" + PlayerPrefs.GetInt("Remix", 0).ToString();
    }

    public void SetBestScore(bool isClear = false)
    {
        if (isClear && SceneManager.GetActiveScene().name.Contains("stage", StringComparison.OrdinalIgnoreCase))
            SetStageBestScore();
        else if (SceneManager.GetActiveScene().name.Contains("remix", StringComparison.OrdinalIgnoreCase))
            SetRemixBestScore();
    }

    private void SetStageBestScore()
    {
        PlayerPrefs.SetInt("Stage", int.Parse(scoreText.text.Substring(1, scoreText.text.Length - 1)));
    }

    private void SetRemixBestScore()
    {
        PlayerPrefs.SetInt("Remix", int.Parse(scoreText.text));
    }

    public void ResetScore()
    {
        PlayerPrefs.DeleteAll();
        ShowBestScore();
    }
}
