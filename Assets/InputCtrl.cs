using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class InputCtrl : MonoBehaviour
{
    public CaterpillarCtrl caterpillarCtrl;

    private HashSet<string> pressedKeys = new HashSet<string>(); // ���� ���� Ű���� ������ ����

    public void KeyStateChange(InputAction.CallbackContext context)
    {
        string keyName = context.control.name;

        if (context.phase == InputActionPhase.Performed)
        {
            if (pressedKeys.Count == 0) caterpillarCtrl.TurnStart();
            pressedKeys.Add(keyName);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            pressedKeys.Remove(keyName);
            if (pressedKeys.Count == 0) caterpillarCtrl.TurnEnd();
        }
    }

    public Vector2 GetMove()
    {
        Vector3 moveDirection = Vector3.zero;

        // ���� ����: W, A, S, D �� ����Ű �Է��� ����
        if (pressedKeys.Contains("w") || pressedKeys.Contains("upArrow"))
        {
            moveDirection.y += 1;
        }
        if (pressedKeys.Contains("s") || pressedKeys.Contains("downArrow"))
        {
            moveDirection.y -= 1;
        }
        if (pressedKeys.Contains("a") || pressedKeys.Contains("leftArrow"))
        {
            moveDirection.x -= 1;
        }
        if (pressedKeys.Contains("d") || pressedKeys.Contains("rightArrow"))
        {
            moveDirection.x += 1;
        }

        // ���� ����ȭ
        return moveDirection.normalized;
    }
}
