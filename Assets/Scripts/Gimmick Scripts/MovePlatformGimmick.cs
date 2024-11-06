using UnityEngine;
using System;

public class MovePlatformGimmick : MonoBehaviour
{
    [SerializeField] private Transform vertex1;
    [SerializeField] private Transform vertex2;
    [SerializeField] private float speed;

    private Vector3 displacement;
    private float deltaX; // x�� �̵� �ִ밪
    private float deltaY; // y�� �̵� �ִ밪
    private int dirX; // x�� �̵� ����
    private int dirY; // y�� �̵� ����

    private Vector3 vertexAveragePos;

    void Start()
    {
        displacement = vertex2.position - vertex1.position; // ���� ���
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
}
