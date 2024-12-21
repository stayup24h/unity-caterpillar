using System.Collections;
using UnityEngine;

public class Spine : MonoBehaviour
{
    static public float interval;

    public bool Head, Tail;

    public GameObject front;
    public GameObject back;
    Transform target;

    public bool isBetween;

    bool isCollision;

    CircleCollider2D circleCollider;
    DistanceJoint2D distanceJoint;
    DistanceJoint2D distanceJoint_HeadTail;

    Coroutine chainingCoroutine;
    Coroutine fixCoroutine;


    void Awake()
    {
        interval = 0.5f;
        circleCollider = GetComponent<CircleCollider2D>();
        DistanceJoint2D[] joints = GetComponents<DistanceJoint2D>();
        if(joints.Length > 1)
        {
            distanceJoint_HeadTail = joints[1];
        }

        distanceJoint = joints[0];
        distanceJoint.distance = interval;
    }

    void Start()
    {
        if(distanceJoint_HeadTail != null) distanceJoint_HeadTail.enabled = false;
        if(front != null) target = front.transform;

        chainingCoroutine = StartCoroutine(Chaining());
    }

    IEnumerator Chaining()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (target == null)
            {
                continue;
            }

            Vector3 direction = (target.position - transform.position).normalized;
            transform.position = target.transform.position - direction * interval;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Object")
        {
            isCollision = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Object")
        {
            isCollision = false;
        }
    }

    public void TurnChange(TurnEnum turn)
    {
        if (turn == TurnEnum.waitHead)
        {
            target = front.transform;
            distanceJoint.connectedBody = front.GetComponent<Rigidbody2D>();
        }
        else
        {
            if(back != null)
            {
                target = back.transform;
                distanceJoint.connectedBody = back.GetComponent<Rigidbody2D>();
            }
        }
    }

    public void TurnChange_Head(TurnEnum turn)
    {
        if (turn == TurnEnum.waitHead)         //헤드턴일때
        {
            distanceJoint.enabled = false;
            distanceJoint_HeadTail.enabled = false;
            if (chainingCoroutine != null)
                StopCoroutine(chainingCoroutine);
        }
        else if(turn == TurnEnum.waitTail)                   //헤드턴이 아닐때
        {
            distanceJoint.enabled = true;
            distanceJoint_HeadTail.enabled = true;
            if (chainingCoroutine == null)
                chainingCoroutine = StartCoroutine(Chaining());
        }
    }

    public void TurnChange_Tail(TurnEnum turn)
    {
        if (turn == TurnEnum.waitTail)
        {
            distanceJoint.enabled = false;
            distanceJoint_HeadTail.enabled = false;
            if (chainingCoroutine != null)
                StopCoroutine(chainingCoroutine);
        }
        else if (turn == TurnEnum.waitHead)
        {
            distanceJoint.enabled = true;
            distanceJoint_HeadTail.enabled = true;
            if (chainingCoroutine == null)
                chainingCoroutine = StartCoroutine(Chaining());
        }
    }
}
