using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StageSelectButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int stageNumber; // Inspector���� ���� ������ �������� ��ȣ

    public void OnPointerClick(PointerEventData eventData)
    {
        string sceneName = "Stage " + stageNumber.ToString("D2"); // �� �̸�: Stage 01, Stage 02 ...
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName); // �� �ε�
        }
        else
        {
            Debug.LogWarning($"Scene '{sceneName}' is not available. Please check the Build Settings.");
        }
    }
}
