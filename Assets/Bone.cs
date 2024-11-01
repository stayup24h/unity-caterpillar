using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bone : MonoBehaviour
{
    public Transform[] targets;           // 이동할 목표 오브젝트들
    public float[] maxDistances;          // 각 타겟에 대한 최대 허용 거리
    public float moveSpeed = 10.0f;       // 이동 속도
    public float upForceScale, rightForceScale;            // 상향 이동에 필요한 힘

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 타겟이 설정되지 않은 경우 경고 메시지
        if (targets == null || targets.Length == 0)
        {
            Debug.LogWarning("Targets이 설정되지 않았습니다.");
            return;
        }

        // 각 타겟에 대해 거리를 계산하고 최대 거리를 초과하면 이동
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] == null) continue; // 타겟이 null이면 무시

            // 현재 타겟과의 거리 계산
            float currentDistance = Vector2.Distance(transform.position, targets[i].position);

            // 최대 거리를 초과했을 때 해당 타겟 방향으로 이동
            if (currentDistance > maxDistances[i] + 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, targets[i].position, moveSpeed * Time.deltaTime);
            }
        }

        // LarvaCtrl 상태에 따라 중력 스케일 조정
        if (CaterpillarCtrl.state == CaterpillarCtrl.State.head || CaterpillarCtrl.state == CaterpillarCtrl.State.tail)
        {
            rb.gravityScale = 0f;
        }
        else if (CaterpillarCtrl.state == CaterpillarCtrl.State.none || CaterpillarCtrl.state == CaterpillarCtrl.State.wait)
        {
            rb.gravityScale = 0.05f;
        }

        // 상향 이동을 위한 힘 추가
        if (CaterpillarCtrl.state == CaterpillarCtrl.State.tail)
        {
            // 타겟을 바라보는 방향으로 회전
            Vector2 direction = targets[0].position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // 현재 오브젝트의 위쪽 방향(transform.up)으로 이동
            Vector3 movement = (transform.up * upForceScale + transform.right * rightForceScale )* Time.deltaTime;
            transform.position += movement;
        }
    }
}
