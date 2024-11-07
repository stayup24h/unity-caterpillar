using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    TextMeshProUGUI ScoreText;
    public GameObject ScoreIndicator;

    void Awake()
    {
        ScoreText = ScoreIndicator.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
//        ScoreText.text = ;
       // 여기에 글자 업뎃 코드 넣기
    }
}
