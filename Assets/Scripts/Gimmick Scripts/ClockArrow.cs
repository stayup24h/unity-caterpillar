using UnityEngine;

public class ClockArrow : MonoBehaviour
{
    private CaterpillarCtrl caterpillarCtrl;
    private Head_Tail head;
    private Head_Tail tail;
    private Rigidbody2D head_rb;
    private Rigidbody2D tail_rb;
    private float prevZ;
    private CameraCtrl cameraCtrl;

    private bool headAttached;
    private bool tailAttached;
    private bool headMoved;
    private bool tailMoved;

    void Start()
    {
        Transform caterpillar = GameObject.Find("Caterpillar").transform;
        caterpillarCtrl = caterpillar.GetComponent<CaterpillarCtrl>();
        head = caterpillar.GetChild(0).GetComponent<Head_Tail>();
        tail = caterpillar.GetChild(6).GetComponent<Head_Tail>();
        head_rb = head.GetComponent<Rigidbody2D>();
        tail_rb = tail.GetComponent<Rigidbody2D>();
        prevZ = transform.rotation.z;
        cameraCtrl = Camera.main.GetComponent<CameraCtrl>();
    }

    void FixedUpdate()
    {
        Vector3 headOffset = Quaternion.Inverse(transform.rotation) * (head.transform.position - transform.position);
        Vector3 tailOffset = Quaternion.Inverse(transform.rotation) * (tail.transform.position - transform.position);

        Vector3 headPos = transform.position + transform.rotation * headOffset;
        Vector3 tailPos = transform.position + transform.rotation * tailOffset;

        if (headAttached && tailAttached)
        {
            head.transform.position = headPos;
            tail.transform.position = tailPos;
            cameraCtrl.MoveCameraToMidPos();
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
                    head.transform.position = headPos;
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
                    tail.transform.position = tailPos;
                    if (!headMoved)
                    {
                        head_rb.constraints = RigidbodyConstraints2D.None;
                        tail_rb.constraints = RigidbodyConstraints2D.FreezeAll;
                        headAttached = false;
                    }
                }
            }
            cameraCtrl.MoveCameraToMidPos();
        }
        prevZ = transform.rotation.z;
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
