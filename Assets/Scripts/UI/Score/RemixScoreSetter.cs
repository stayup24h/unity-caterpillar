using UnityEngine;

public class RemixScoreSetter : ScoreSetter
{
    [SerializeField] private bool isNeedColon;

    protected override void SetScore()
    {
        score = PlayerPrefs.GetInt("Remix", 0);
        scoreText.text = "BEST" + (isNeedColon ? ": " : " ") + ((score / 100 == 0) ? "0" : "") + ((score / 10 == 0) ? "0" : "") + score.ToString();
    }
}
