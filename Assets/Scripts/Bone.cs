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
    public Transform[] targets;           // �̵��� ��ǥ ������Ʈ��
    public float[] maxDistances;          // �� Ÿ�ٿ� ���� �ִ� ��� �Ÿ�
    public float moveSpeed = 10.0f;       // �̵� �ӵ�
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

        // �� Ÿ�ٿ� ���� �Ÿ��� ����ϰ� �ִ� �Ÿ��� �ʰ��ϸ� �̵�
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] == null) continue; // Ÿ���� null�̸� ����

            // ���� Ÿ�ٰ��� �Ÿ� ���
            float currentDistance = Vector2.Distance(transform.position, targets[i].position);

            // �ִ� �Ÿ��� �ʰ����� �� �ش� Ÿ�� �������� �̵�
            if (currentDistance > maxDistances[i] + 0.1f)
            {
                movePosition += (targets[i].position - transform.position).normalized * moveSpeed * Time.deltaTime;
                //transform.position = Vector2.MoveTowards(transform.position, targets[i].position, moveSpeed * Time.deltaTime);
            }
        } 

        // LarvaCtrl ���¿� ���� �߷� ������ ����
        if (caterpillarCtrl.state == CaterpillarCtrl.State.head || caterpillarCtrl.state == CaterpillarCtrl.State.tail)
        {
            rb.gravityScale = 0f;
        }
        else if (caterpillarCtrl.state == CaterpillarCtrl.State.none || caterpillarCtrl.state == CaterpillarCtrl.State.wait)
        {
            rb.gravityScale = 0.1f;
        }

        // ���� �̵��� ���� �̵� �߰�
        if (caterpillarCtrl.state == CaterpillarCtrl.State.tail || caterpillarCtrl.state == CaterpillarCtrl.State.wait)
        {
            // Ÿ���� �ٶ󺸴� �������� ȸ��
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
