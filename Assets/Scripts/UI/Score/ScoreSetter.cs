using TMPro;
using UnityEngine;

public abstract class ScoreSetter : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI scoreText;
    protected int score = 0;

    protected abstract void SetScore();

    private void Awake()
    {
        SetScore();
    }
}
