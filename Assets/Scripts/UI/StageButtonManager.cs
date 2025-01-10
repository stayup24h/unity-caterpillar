using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageButtonManager : MonoBehaviour
{
    [SerializeField] private Transform stage;

    private int maxStage = 15;
    void Awake()
    {
        int stageScore = PlayerPrefs.GetInt("Stage", 0);
        foreach (Transform child in stage)
        {
            child.GetChild(0).GetComponent<Button>().interactable = false;
        }
        for (int i = 0; i < stageScore; i++)
        {
            int stageNum = i;
            Button button = SetButtonSetting(stageNum);
            button.transform.GetChild(0).gameObject.SetActive(true);
        }
        if (stageScore != maxStage)
        {
            SetButtonSetting(stageScore);
        }
    }

    private Button SetButtonSetting(int stageNum)
    {
        Button button = stage.GetChild(stageNum).GetChild(0).GetComponent<Button>();
        button.interactable = true;
        button.onClick.AddListener(() => LoadStageScene(stageNum + 1));

        return button;
    }

    private void LoadStageScene(int stageNum)
    {
        GameManager.Instance.StageNum = stageNum;
        SceneManager.LoadScene("InGame");
    }
}
