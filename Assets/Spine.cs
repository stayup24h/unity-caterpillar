using System.Collections;
using UnityEngine;

public class Spine : MonoBehaviour
{
    static public float interval;
    public bool isHeadTurn;

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
        isHeadTurn = GameManager.isHeadTurn;

        fixCoroutine = StartCoroutine(Fix());
        chainingCoroutine = StartCoroutine(Chaining());
    }

    void FixedUpdate()
    {
        if (isHeadTurn != GameManager.isHeadTurn)
        {
            isHeadTurn = GameManager.isHeadTurn;
            TurnChange(isHeadTurn);
        }
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

    IEnumerator Fix()
    {
        while(isCollision != false)
        {
            yield return new WaitForFixedUpdate();
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

    void TurnChange(bool isHeadTurn)
    {
        print("턴 바뀜     헤드 턴: " + isHeadTurn);

        if (isBetween)
        {
            if (isHeadTurn)
            {
                target = front.transform;
                distanceJoint.connectedBody = front.GetComponent<Rigidbody2D>();
            }
            else
            {
                target = back.transform;
                distanceJoint.connectedBody = back.GetComponent<Rigidbody2D>();
            }
        }


        else if (Head)
        {
            if (isHeadTurn)         //헤드턴일때
            {
                distanceJoint.enabled = false;
                distanceJoint_HeadTail.enabled = false;
                if (chainingCoroutine != null) 
                    StopCoroutine(chainingCoroutine);
            }
            else                    //헤드턴이 아닐때
            {
                distanceJoint.enabled = true;
                distanceJoint_HeadTail.enabled = true;
                if (chainingCoroutine == null)
                    chainingCoroutine = StartCoroutine(Chaining());
            }
        }


        else if (Tail)
        {
            if (!isHeadTurn)        //헤드턴이 아닐때
            {
                distanceJoint.enabled = false;
                distanceJoint_HeadTail.enabled = false;
                if (chainingCoroutine != null) 
                    StopCoroutine(chainingCoroutine);
            }
            else                    //헤드턴일때
            {
                distanceJoint.enabled = true;
                distanceJoint_HeadTail.enabled = true;
                if (chainingCoroutine == null)
                    chainingCoroutine = StartCoroutine(Chaining());
            }
        }
    }
}
