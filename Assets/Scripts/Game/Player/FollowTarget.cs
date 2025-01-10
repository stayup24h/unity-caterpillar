using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform frontTarget;
    public Transform backTarget;

    Vector3 testVector;
    Vector3 moveVector;
    Transform target;
    Transform follower;

    public void Awake()
    {
        target = frontTarget;
        if(backTarget != null ) follower = backTarget;
    }

    public void ChangeTarget(bool isFront)
    {
        if (backTarget != null)
        {
            target = (isFront) ? frontTarget : backTarget;
            follower = (!isFront) ? frontTarget : backTarget;
        }
    }

    public bool Check(Vector3 move)
    {
        moveVector = target.position + move - transform.position;
        testVector = transform.position + moveVector;
        return (Vector3.Distance(testVector, target.position) >= 1f);
    }

    public bool EndCehck()
    {
        testVector = transform.position + moveVector;
        return ((Vector3.Distance(testVector, frontTarget.position) <= 1f) && (Vector3.Distance(testVector, backTarget.position) <= 1f));
    }

    public void Follow()
    {
        transform.position += moveVector;
    }
}
