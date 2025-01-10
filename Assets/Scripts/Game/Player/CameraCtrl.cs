using System.Collections;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public Vector3 offset;   // Ÿ�ٰ��� ������
    public float moveSpeed = 5.0f;

    public bool isRunning_Camera;

    private Transform Caterpillar; // ī�޶� ���� Ÿ�� ������Ʈ
    private Vector3 velocity = Vector3.zero;

    void Awake()
    {
        isRunning_Camera = false;
        Caterpillar = GameObject.Find("Caterpillar").transform;
    }
    void Start()
    {
        MoveCamera();
    }

    public void MoveCamera(bool isHeadTurn = true)
    {
        if (Caterpillar != null && !isRunning_Camera)
        {
            Transform target = isHeadTurn ? Caterpillar.GetChild(0) : Caterpillar.GetChild(6);
            Vector3 targetPosition = target.position + offset;
            StartCoroutine(FollowTarget(targetPosition));
        }
    }

    public void MoveCameraToMidPos()
    {
        if (Caterpillar != null && !isRunning_Camera)
        {
            Vector3 targetPosition = (Caterpillar.GetChild(0).position + Caterpillar.GetChild(6).position) / 2;
            targetPosition = new Vector3(Mathf.FloorToInt(targetPosition.x), Mathf.FloorToInt(targetPosition.y), Mathf.FloorToInt(targetPosition.z)) + offset;
            StartCoroutine(FollowTarget(targetPosition));
        }
    }

    IEnumerator FollowTarget(Vector3 targetPosition)
    {
        isRunning_Camera = true;
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            // Lerp�� ����Ͽ� Ÿ�� ��ġ�� �ε巴�� �̵�
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        
            // �� ������ ���
            yield return null;
        }
        isRunning_Camera = false;
    }
}
