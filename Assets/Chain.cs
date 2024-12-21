using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Chain : MonoBehaviour
{
    DistanceJoint2D distanceJoint;
    CircleCollider2D circleCollider;
    public float radius;
    public bool fixRotation;

    public GameObject front;
    public GameObject back;
    GameObject target;
    Rigidbody2D targetRigidbody;



    Coroutine chainCoroutine;
    void Awake()
    {
        distanceJoint = GetComponent<DistanceJoint2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        distanceJoint.distance = radius * 2;

        if (front != null)
            target = front;
        else if (back != null)
            target = back;
        else return;
        targetRigidbody = target.GetComponent<Rigidbody2D>();
        distanceJoint.connectedBody = targetRigidbody;

        StartChain();
    }

    public void StartChain()
    {
        if (chainCoroutine == null)
            chainCoroutine = StartCoroutine(Chaining());
    }

    [ContextMenu("stop")]
    public void StopChain()
    {
        if(chainCoroutine != null)
            StopCoroutine(chainCoroutine);
    }

    public void ChangeTarget(bool isFront)
    {
        if (isFront && front != null)
        {
            target = front;
            targetRigidbody = target.GetComponent<Rigidbody2D>();
            distanceJoint.connectedBody = targetRigidbody;
        }
        else if (!isFront && back != null)
        {
            target = back;
            targetRigidbody = target.GetComponent<Rigidbody2D>();
            distanceJoint.connectedBody = targetRigidbody;
        }
    }

    IEnumerator Chaining()
    {
        while (true)
        {
            // ���� ���� ���
            Vector3 direction = (target.transform.position - transform.position).normalized;

            // ��ü�� target �������� ȸ�� (2D���� Z�� ����)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if(fixRotation) 
                transform.rotation = Quaternion.Euler(0, 0, angle);

            // ��ġ ������Ʈ
            transform.position = target.transform.position - direction * radius * 2;

            yield return new WaitForFixedUpdate();
        }
    }
}
