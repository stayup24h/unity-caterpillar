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
    private Vector3 vertexAveragePos;

    private CaterpillarCtrl caterpillarCtrl;
    private Head_Tail head;
    private Head_Tail tail;
    private Rigidbody2D head_rb;
    private Rigidbody2D tail_rb;
    private Vector3 prevPosition;
    private CameraCtrl cameraCtrl;

    private bool headAttached;
    private bool tailAttached;
    private bool headMoved;
    private bool tailMoved;

    void Start()
    {
        displacement = vertex2.position - vertex1.position; // 변위 계산
        deltaX = displacement.x / 2;
        deltaY = displacement.y / 2;
        vertexAveragePos = (vertex1.position + vertex2.position) / 2;

        Transform caterpillar = GameObject.Find("Caterpillar").transform;
        caterpillarCtrl = caterpillar.GetComponent<CaterpillarCtrl>();
        head = caterpillar.GetChild(0).GetComponent<Head_Tail>();
        tail = caterpillar.GetChild(6).GetComponent<Head_Tail>();
        head_rb = head.GetComponent<Rigidbody2D>();
        tail_rb = tail.GetComponent<Rigidbody2D>();
        prevPosition = transform.position;

        cameraCtrl = Camera.main.GetComponent<CameraCtrl>();
    }

    void FixedUpdate()
    {
        float delta = Mathf.Sin(Time.time / displacement.magnitude * speed - Mathf.PI / 2);
        transform.position = vertexAveragePos + new Vector3(delta * deltaX, delta * deltaY, 0);

        Vector3 deltaMove = transform.position - prevPosition;

        if (headAttached && tailAttached)
        {
            head.transform.position += deltaMove;
            tail.transform.position += deltaMove;
            cameraCtrl.Start();
        }
        else if (headAttached || tailAttached)
        {
            if (!caterpillarCtrl.IsRunning_head)
            {
                if (headMoved)
                {
                    head_rb.constraints = RigidbodyConstraints2D.FreezeAll;
                    tail_rb.constraints = RigidbodyConstraints2D.None;
                    tailAttached = false;
                    if (head.attachedObject == gameObject) headAttached = true;
                }
                if (headAttached)
                {
                    head.transform.position += deltaMove;
                    if (!(headMoved || tailMoved))
                    {
                        head_rb.constraints = RigidbodyConstraints2D.FreezeAll;
                        tail_rb.constraints = RigidbodyConstraints2D.None;
                        tailAttached = false;
                    }
                }
            }
            if (!caterpillarCtrl.IsRunning_tail)
            {
                if (tailMoved)
                {
                    head_rb.constraints = RigidbodyConstraints2D.None;
                    tail_rb.constraints = RigidbodyConstraints2D.FreezeAll;
                    headAttached = false;
                    if (tail.attachedObject == gameObject) tailAttached = true;
                }
                if (tailAttached)
                {
                    tail.transform.position += deltaMove;
                    if (!headMoved)
                    {
                        head_rb.constraints = RigidbodyConstraints2D.None;
                        tail_rb.constraints = RigidbodyConstraints2D.FreezeAll;
                        headAttached = false;
                    }
                }
            }
            cameraCtrl.Start();
        }
        prevPosition = transform.position;
        if (caterpillarCtrl.IsRunning_head) headMoved = true;
        else headMoved = false;
        if (caterpillarCtrl.IsRunning_tail) tailMoved = true;
        else tailMoved = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Head") && (caterpillarCtrl.state != CaterpillarCtrl.State.head || !caterpillarCtrl.IsRunning_head))
        {
            head_rb.constraints = RigidbodyConstraints2D.FreezeAll;
            tail_rb.constraints = RigidbodyConstraints2D.None;
            headAttached = true;
        }
        if (collision.gameObject.name.Contains("Tail") && (caterpillarCtrl.state != CaterpillarCtrl.State.tail || !caterpillarCtrl.IsRunning_tail))
        {
            head_rb.constraints = RigidbodyConstraints2D.None;
            tail_rb.constraints = RigidbodyConstraints2D.FreezeAll;
            tailAttached = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Head") && headMoved && (caterpillarCtrl.state != CaterpillarCtrl.State.head || !caterpillarCtrl.IsRunning_head)) headAttached = true;
        if (collision.gameObject.name.Contains("Tail") && tailMoved && (caterpillarCtrl.state != CaterpillarCtrl.State.tail || !caterpillarCtrl.IsRunning_head)) tailAttached = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Head")) headAttached = false;
        if (collision.gameObject.name.Contains("Tail")) tailAttached = false;
    }
}
