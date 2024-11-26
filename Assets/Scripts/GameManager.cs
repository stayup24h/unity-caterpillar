using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public bool isHeadTurn { get; private set; }

    void Awake()
    {
        isHeadTurn = true;
    }

    [ContextMenu("CCCCC")]
    public void Change()
    {
        isHeadTurn = !isHeadTurn;
        print("ео Change " + isHeadTurn);
    }
}
