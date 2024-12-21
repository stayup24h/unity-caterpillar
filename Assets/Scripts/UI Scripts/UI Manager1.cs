using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StageButtonScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private string stageName; // ��ư���� �������� �̸��� ����

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(stageName))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(stageName);
        }
        else
        {
            Debug.LogWarning("Stage name is not set for this button.");
        }
    }
}