using System.Collections;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public Transform target; // ī�޶� ���� Ÿ�� ������Ʈ
    public Vector3 offset;   // Ÿ�ٰ��� ������
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
            // �ڷ�ƾ ����
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
                // Lerp�� ����Ͽ� Ÿ�� ��ġ�� �ε巴�� �̵�
                transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
            // �� ������ ���
            yield return null;
        }
        isRunning_Camera = false;
    }
}
