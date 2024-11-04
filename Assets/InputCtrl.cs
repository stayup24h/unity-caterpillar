using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class InputCtrl : MonoBehaviour
{
    public CaterpillarCtrl caterpillarCtrl;

    private HashSet<string> pressedKeys = new HashSet<string>(); // 현재 눌린 키들을 저장할 집합
    private bool anyKeyPressed = false; // 모든 키가 떼어진 상태인지 여부를 나타내는 플래그

    // 확인할 키 목록 정의 (Input System에서 사용하는 Control Path)
    private readonly Dictionary<string, string> targetKeys = new Dictionary<string, string>
    {
        { "w", "<Keyboard>/w" },
        { "a", "<Keyboard>/a" },
        { "s", "<Keyboard>/s" },
        { "d", "<Keyboard>/d" },
        { "upArrow", "<Keyboard>/upArrow" },
        { "downArrow", "<Keyboard>/downArrow" },
        { "leftArrow", "<Keyboard>/leftArrow" },
        { "rightArrow", "<Keyboard>/rightArrow" }
    }; 

    void Awake()
    {
        anyKeyPressed = false;
    }

    public void KeyStateChange(InputAction.CallbackContext context)
    {
        string keyName = context.control.name;

        if (context.phase == InputActionPhase.Performed)
        {
            if (pressedKeys.Count == 0) caterpillarCtrl.TurnStart();
            pressedKeys.Add(keyName);
            

            Debug.Log(keyName + " :눌림,  " + pressedKeys.Count);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            pressedKeys.Remove(keyName);
            if (pressedKeys.Count == 0) caterpillarCtrl.TurnEnd();
            Debug.Log(keyName + " :뗌,  " + pressedKeys.Count);
            
        }
    }
}
