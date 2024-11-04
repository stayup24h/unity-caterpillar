using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class InputCtrl : MonoBehaviour
{
    public CaterpillarCtrl caterpillarCtrl;

    private HashSet<string> pressedKeys = new HashSet<string>(); // 현재 눌린 키들을 저장할 집합

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

        // 방향 설정: W, A, S, D 및 방향키 입력을 감지
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

        // 벡터 정규화
        return moveDirection.normalized;
    }
}
