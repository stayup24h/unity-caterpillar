using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    [SerializeField] private Transform mapPool;
    [SerializeField] private Transform caterpillarHead;
    [SerializeField] private TextMeshProUGUI scoreText;

    private Transform prevScorePoint;

    void Awake()
    {
        if (caterpillarHead == null) caterpillarHead = GameObject.Find("Caterpillar").transform.GetChild(0);
    }

    void Update()
    {
        Transform currentScorePoint = mapPool.GetChild(1).Find("Start Point");
        if (prevScorePoint != currentScorePoint)
        {
            if (caterpillarHead.position.x > currentScorePoint.position.x)
            {
                scoreText.text = (int.Parse(scoreText.text)+1).ToString();
                GameManager.Instance.ChangeSoundType();
                prevScorePoint = currentScorePoint;
            }
        }
    }
}
