using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class InputCtrl : MonoBehaviour
{
    public CaterpillarCtrl caterpillarCtrl;

    private HashSet<string> pressedKeys = new HashSet<string>(); // ���� ���� Ű���� ������ ����
    private bool anyKeyPressed = false; // ��� Ű�� ������ �������� ���θ� ��Ÿ���� �÷���

    // Ȯ���� Ű ��� ���� (Input System���� ����ϴ� Control Path)
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
            

            Debug.Log(keyName + " :����,  " + pressedKeys.Count);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            pressedKeys.Remove(keyName);
            if (pressedKeys.Count == 0) caterpillarCtrl.TurnEnd();
            Debug.Log(keyName + " :��,  " + pressedKeys.Count);
            
        }
    }
}
