using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    [SerializeField] private GameObject stageMode;
    [SerializeField] private GameObject remixMode;

    void Start()
    {
        if (GameManager.Instance.MapType == MapType.stage)
        {
            stageMode.SetActive(true);
            remixMode.SetActive(false);
        }
        else if (GameManager.Instance.MapType == MapType.remix)
        {
            stageMode.SetActive(false);
            remixMode.SetActive(true);
        }
    }
}
