using UnityEngine;

public class StageScoreSetter : ScoreSetter
{
    protected override void SetScore()
    {
        score = PlayerPrefs.GetInt("Stage", 0) + 1;
        scoreText.text = "#" + ((score / 10 == 0) ? "0" : "") + score.ToString();
    }
}
