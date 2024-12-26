using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StageSelectButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int stageNumber; // Inspector에서 설정 가능한 스테이지 번호

    public void OnPointerClick(PointerEventData eventData)
    {
        string sceneName = "Stage " + stageNumber.ToString("D2"); // 씬 이름: Stage 01, Stage 02 ...
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName); // 씬 로드
        }
        else
        {
            Debug.LogWarning($"Scene '{sceneName}' is not available. Please check the Build Settings.");
        }
    }
}
