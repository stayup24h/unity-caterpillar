using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bone : MonoBehaviour
{
    public Transform[] targets;           // �̵��� ��ǥ ������Ʈ��
    public float[] maxDistances;          // �� Ÿ�ٿ� ���� �ִ� ��� �Ÿ�
    public float moveSpeed = 10.0f;       // �̵� �ӵ�
    public float upForceScale, rightForceScale;            // ���� �̵��� �ʿ��� ��

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Ÿ���� �������� ���� ��� ��� �޽���
        if (targets == null || targets.Length == 0)
        {
            Debug.LogWarning("Targets�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // �� Ÿ�ٿ� ���� �Ÿ��� ����ϰ� �ִ� �Ÿ��� �ʰ��ϸ� �̵�
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] == null) continue; // Ÿ���� null�̸� ����

            // ���� Ÿ�ٰ��� �Ÿ� ���
            float currentDistance = Vector2.Distance(transform.position, targets[i].position);

            // �ִ� �Ÿ��� �ʰ����� �� �ش� Ÿ�� �������� �̵�
            if (currentDistance > maxDistances[i] + 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, targets[i].position, moveSpeed * Time.deltaTime);
            }
        }

        // LarvaCtrl ���¿� ���� �߷� ������ ����
        if (CaterpillarCtrl.state == CaterpillarCtrl.State.head || CaterpillarCtrl.state == CaterpillarCtrl.State.tail)
        {
            rb.gravityScale = 0f;
        }
        else if (CaterpillarCtrl.state == CaterpillarCtrl.State.none || CaterpillarCtrl.state == CaterpillarCtrl.State.wait)
        {
            rb.gravityScale = 0.05f;
        }

        // ���� �̵��� ���� �� �߰�
        if (CaterpillarCtrl.state == CaterpillarCtrl.State.tail)
        {
            // Ÿ���� �ٶ󺸴� �������� ȸ��
            Vector2 direction = targets[0].position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // ���� ������Ʈ�� ���� ����(transform.up)���� �̵�
            Vector3 movement = (transform.up * upForceScale + transform.right * rightForceScale )* Time.deltaTime;
            transform.position += movement;
        }
    }
}
