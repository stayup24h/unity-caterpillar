using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject settingPanel;

    public void OpenSettingPanel()
    {
        Time.timeScale = 0;
        settingPanel.SetActive(true);
    }

    public void CloseSettingPanel()
    {
        Time.timeScale = 1;
        settingPanel.SetActive(false);
    }

    public void Retry()
    {
        CloseSettingPanel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMain()
    {
        CloseSettingPanel();
        SceneManager.LoadScene("Main");
    }

    public void GoToNextStage()
    {
        CloseSettingPanel();
        GameManager.Instance.StageNum++;
        Retry();
    }

    public void ResetScore()
    {
        PlayerPrefs.DeleteAll();
    }
}
