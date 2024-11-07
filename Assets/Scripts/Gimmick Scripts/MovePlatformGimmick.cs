using UnityEngine;
using System;

public class MovePlatformGimmick : MonoBehaviour
{
    [SerializeField] private Transform vertex1;
    [SerializeField] private Transform vertex2;
    [SerializeField] private float speed;

    private Vector3 displacement;
    private float deltaX; // x축 이동 최대값
    private float deltaY; // y축 이동 최대값
    private int dirX; // x축 이동 방향
    private int dirY; // y축 이동 방향
    private Vector3 vertexAveragePos;

    private Transform caterpillar;
    private Head_Tail head;
    private Head_Tail tail;
    private Rigidbody2D head_rb;
    private Rigidbody2D tail_rb;
    private Vector3 prevPosition;

    void Start()
    {
        displacement = vertex2.position - vertex1.position; // 변위 계산
        deltaX = displacement.x / 2;
        deltaY = displacement.y / 2;
        dirX = Math.Sign(deltaX);
        dirY = Math.Sign(deltaY);
        vertexAveragePos = (vertex1.position + vertex2.position) / 2;

        caterpillar = GameObject.Find("Caterpillar").transform;
        head = caterpillar.GetChild(0).GetComponent<Head_Tail>();
        tail = caterpillar.GetChild(6).GetComponent<Head_Tail>();
        head_rb = head.GetComponent<Rigidbody2D>();
        tail_rb = tail.GetComponent<Rigidbody2D>();
        prevPosition = transform.position;
    }

    void Update()
    {
        float delta = Mathf.Sin(Time.time / displacement.magnitude * speed - Mathf.PI / 2);
        transform.position = vertexAveragePos + new Vector3(delta * deltaX, delta * deltaY, 0);

        Vector3 deltaMove = transform.position - prevPosition;
        if (head.attachedObject == gameObject)
        {
            head.transform.position += deltaMove;
        }
        if (tail.attachedObject == gameObject)
        {
            tail.transform.position += deltaMove;
        }
        prevPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.parent.CompareTag("Player"))
        {
            if (head.attachedObject != gameObject)
            {
                head_rb.constraints = RigidbodyConstraints2D.None;
            }
            if (tail.attachedObject != gameObject)
            {
                tail_rb.constraints = RigidbodyConstraints2D.None;
            }
        }
    }
}
