using UnityEngine;

public class BlinkPlatform : MonoBehaviour
{
    private Transform caterpillar;
    private Head_Tail head;
    private Head_Tail tail;
    private Rigidbody2D head_rb;
    private Rigidbody2D tail_rb;

    void Awake()
    {
        caterpillar = GameObject.Find("Caterpillar").transform;
        head = caterpillar.GetChild(0).GetComponent<Head_Tail>();
        tail = caterpillar.GetChild(6).GetComponent<Head_Tail>();
        head_rb = head.GetComponent<Rigidbody2D>();
        tail_rb = tail.GetComponent<Rigidbody2D>();
    }

    public void PlatformDisappear()
    {
        if (head != null && ReferenceEquals(head.attachedObject, gameObject))
        {
            head_rb.constraints = RigidbodyConstraints2D.None;
        }
        if (tail != null && ReferenceEquals(tail.attachedObject, gameObject))
        {
            tail_rb.constraints = RigidbodyConstraints2D.None;
        }
    }
}
