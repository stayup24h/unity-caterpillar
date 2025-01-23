using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    public bool isHeadTurn { get; private set; }
    public MapType MapType { get; set; }
    public int StageNum { get; set; } = 1;

    public List<GameObject> mapPatterns;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        isHeadTurn = true;

        mapPatterns = new List<GameObject>(Resources.LoadAll<GameObject>("Prefabs/Map Prefab"));
    }
    

    [ContextMenu("CCCCC")]
    public void Change()
    {
        isHeadTurn = !isHeadTurn;
        print("ео Change " + isHeadTurn);
    }
}
