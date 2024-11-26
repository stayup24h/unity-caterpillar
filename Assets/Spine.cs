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

    CircleCollider2D circleCollider;
    DistanceJoint2D distanceJoint;

    Coroutine chainingCoroutine;
    Coroutine fixCoroutine;


    void Awake()
    {
        interval = 0.5f;
        circleCollider = GetComponent<CircleCollider2D>();
        distanceJoint = GetComponent<DistanceJoint2D>();

        distanceJoint.distance = interval;
    }

    void Start()
    {
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
            TurnChange();
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
        yield return new WaitForFixedUpdate();
    }

    void TurnChange()
    {
        print("턴 바뀜     헤드 턴: " + GameManager.isHeadTurn);

        if (isBetween)
        {
            if (GameManager.isHeadTurn)
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
            if (GameManager.isHeadTurn)
            {
                distanceJoint.enabled = false;
                if(chainingCoroutine != null) 
                    StopCoroutine(chainingCoroutine);
            }
            else
            {
                distanceJoint.enabled = true;
                if (chainingCoroutine == null)
                    chainingCoroutine = StartCoroutine(Chaining());
            }
        }


        else if (Tail)
        {
            if (!GameManager.isHeadTurn)
            {
                distanceJoint.enabled = false;
                if (chainingCoroutine != null) 
                    StopCoroutine(chainingCoroutine);
            }
            else
            {
                distanceJoint.enabled = true;
                if (chainingCoroutine == null)
                    chainingCoroutine = StartCoroutine(Chaining());
            }
        }
    }
}
