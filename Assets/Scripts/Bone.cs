using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bone : MonoBehaviour
{
    public CaterpillarCtrl caterpillarCtrl;
    public int position;
    public Transform head;
    public Transform tail;
    public Transform[] targets;           // 이동할 목표 오브젝트들
    public float[] maxDistances;          // 각 타겟에 대한 최대 허용 거리
    public float moveSpeed = 10.0f;       // 이동 속도
    public float upForce;

    Rigidbody2D rb;
    Vector3 lastTailPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lastTailPosition = tail.position;
    }

    void Update()
    {
        Vector3 v = head.position - tail.position;
        Vector3 normalV = new Vector3(-v.y, v.x, 0).normalized;
        Vector3 currentTailPosition = tail.position;
        Vector3 movePosition = transform.position;

        // 각 타겟에 대해 거리를 계산하고 최대 거리를 초과하면 이동
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] == null) continue; // 타겟이 null이면 무시

            // 현재 타겟과의 거리 계산
            float currentDistance = Vector2.Distance(transform.position, targets[i].position);

            // 최대 거리를 초과했을 때 해당 타겟 방향으로 이동
            if (currentDistance > maxDistances[i] + 0.1f)
            {
                movePosition += (targets[i].position - transform.position).normalized * moveSpeed * Time.deltaTime;
                //transform.position = Vector2.MoveTowards(transform.position, targets[i].position, moveSpeed * Time.deltaTime);
            }
        } 

        // LarvaCtrl 상태에 따라 중력 스케일 조정
        if (caterpillarCtrl.state == CaterpillarCtrl.State.head || caterpillarCtrl.state == CaterpillarCtrl.State.tail)
        {
            rb.gravityScale = 0f;
        }
        else if (caterpillarCtrl.state == CaterpillarCtrl.State.none || caterpillarCtrl.state == CaterpillarCtrl.State.wait)
        {
            rb.gravityScale = 0.1f;
        }

        // 상향 이동을 위한 이동 추가
        if (caterpillarCtrl.state == CaterpillarCtrl.State.tail || caterpillarCtrl.state == CaterpillarCtrl.State.wait)
        {
            // 타겟을 바라보는 방향으로 회전
            Vector2 direction = head.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.Euler(0, 0, angle);
            if (Vector3.Distance(head.position, tail.position) <= 5.9f)
                movePosition += normalV * upForce * Time.deltaTime;

            if(position < 6)
            movePosition += (currentTailPosition - lastTailPosition) * 0.1f * position;
        }

        transform.position = movePosition;
        lastTailPosition = tail.position;
    }
}
