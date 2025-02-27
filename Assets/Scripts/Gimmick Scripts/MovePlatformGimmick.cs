using UnityEngine;

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
    private Vector3 prevPosition;
    private CameraCtrl cameraCtrl;

    private bool headAttached;
    private bool tailAttached;

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
            cameraCtrl.MoveCameraToMidPos();
        }
        else if (headAttached || tailAttached)
        {
            if (caterpillarCtrl.fixHead == null)
            {
                if (headAttached)
                {
                    head.transform.position += deltaMove;
                }
            }
            if (caterpillarCtrl.fixTail == null)
            {
                if (tailAttached)
                {
                    tail.transform.position += deltaMove;
                }
            }
            cameraCtrl.MoveCameraToMidPos();
        }

        prevPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Head")) headAttached = true;
        if (collision.gameObject.name.Contains("Tail")) tailAttached = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Head")) headAttached = true;
        if (collision.gameObject.name.Contains("Tail")) tailAttached = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Head")) headAttached = false;
        if (collision.gameObject.name.Contains("Tail")) tailAttached = false;
    }
}
