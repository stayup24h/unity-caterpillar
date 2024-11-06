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
    private bool isAttach;

    void Start()
    {
        displacement = vertex2.position - vertex1.position; // 변위 계산
        deltaX = displacement.x / 2;
        deltaY = displacement.y / 2;
        dirX = Math.Sign(deltaX);
        dirY = Math.Sign(deltaY);
        vertexAveragePos = (vertex1.position + vertex2.position) / 2;
    }

    void Update()
    {
        float delta = Mathf.Sin(Time.time / displacement.magnitude * speed - Mathf.PI / 2);
        transform.position = vertexAveragePos + new Vector3(delta * deltaX, delta * deltaY, 0);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            isAttach = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            isAttach = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            isAttach = false;
    }
}
