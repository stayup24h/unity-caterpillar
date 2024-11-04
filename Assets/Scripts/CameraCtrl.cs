using System.Collections;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public Transform target; // 카메라가 따라갈 타겟 오브젝트
    public Vector3 offset;   // 타겟과의 오프셋
    public float moveSpeed = 5.0f;

    public bool isRunning_Camera;

    private Vector3 velocity = Vector3.zero;

    void Awake()
    {
        isRunning_Camera = false;
    }
    public void Start()
    {
        if (target != null && !isRunning_Camera)
        {
            // 코루틴 시작
            StartCoroutine(FollowTarget());
        }
    }

    IEnumerator FollowTarget()
    {
        isRunning_Camera = true;
        Vector3 targetPosition = target.position + offset;
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            if (target != null)
            {
                // Lerp를 사용하여 타겟 위치로 부드럽게 이동
                transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
            // 한 프레임 대기
            yield return null;
        }
        isRunning_Camera = false;
    }
}
