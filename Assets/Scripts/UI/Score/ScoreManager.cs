using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI scoreText;
    [SerializeField] protected int score;

    protected string key;

    protected void UpdateBestScore()
    {
        Debug.Log("���� ������Ʈ ��" + key + score);
        PlayerPrefs.SetInt(key, Mathf.Max(PlayerPrefs.GetInt(key, 0), score));
    }
}
