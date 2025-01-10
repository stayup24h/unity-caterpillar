using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private MapType mapType;

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.MapType = mapType;
        if (mapType == MapType.stage) GameManager.Instance.StageNum = PlayerPrefs.GetInt("Stage", 0) + 1;
        SceneManager.LoadScene("InGame");
    }
}
