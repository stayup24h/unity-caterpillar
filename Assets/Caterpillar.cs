using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Caterpillar : MonoBehaviour
{
    public enum TurnEnum { wait, head, tail, end}
    public TurnEnum turnState;

    public GameObject[] bodies;
    public GameObject head, tail;

    LineRenderer lineRenderer;

    Chain[] bodyChains = new Chain[9];

    void Awake()
    {
        AddDistanceJoint();
        InitializeLineRenderer();
        for (int i = 0; i < bodies.Length; i++)
        {
            bodyChains[i] = bodies[i].GetComponent<Chain>();
        }

        turnState = TurnEnum.wait;
    }

    void AddDistanceJoint()
    {
        DistanceJoint2D headDistanceJoint = head.AddComponent<DistanceJoint2D>();
        headDistanceJoint.autoConfigureDistance = false;
        headDistanceJoint.maxDistanceOnly = true;
        headDistanceJoint.connectedBody = tail.GetComponent<Rigidbody2D>();
        headDistanceJoint.distance = 3.5f;

        DistanceJoint2D tailDistanceJoint = tail.AddComponent<DistanceJoint2D>();
        tailDistanceJoint.autoConfigureDistance = false;
        tailDistanceJoint.maxDistanceOnly = true;
        tailDistanceJoint.connectedBody = head.GetComponent<Rigidbody2D>();
        tailDistanceJoint.distance = 3.5f;
    }

    void FixedUpdate()
    {
        switch (turnState)
        {
            case TurnEnum.wait:
                {
                    break;
                }
            case TurnEnum.head:
                {
                    break;
                }
            case TurnEnum.tail:
                {
                    break;
                }
            case TurnEnum.end:
                {
                    break;
                }
        }
        
    }

    void LateUpdate()
    {
        UpdateLineRenderer();
    }

    void InitializeLineRenderer()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = bodies.Length * 2 + 3;
        lineRenderer.startWidth = 0.1f; // 선의 시작 두께
        lineRenderer.endWidth = 0.1f;   // 선의 끝 두께
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // 기본 셰이더
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
    }

    void UpdateLineRenderer()
    {
        int newPosition = 0;
        Vector3 newPoint;


        newPoint = head.transform.position + head.transform.right * 0.25f;
        lineRenderer.SetPosition(newPosition++, newPoint);

        for (int i = 0; i < bodies.Length; i++)
        {
            if (bodies[i] != null)
            {
                newPoint = bodies[i].transform.position - bodies[i].transform.up * 0.25f;
                lineRenderer.SetPosition(newPosition++, newPoint);
            }
        }

        newPoint = bodies[bodies.Length - 1].transform.position - bodies[bodies.Length - 1].transform.right * 0.25f;
        lineRenderer.SetPosition(newPosition++, newPoint);

        for (int i = bodies.Length - 1; i >= 0; i--)
        {
            if (bodies[i] != null)
            {
                newPoint = bodies[i].transform.position + bodies[i].transform.up * 0.25f;
                lineRenderer.SetPosition(newPosition++, newPoint);
            }
        }

        newPoint = head.transform.position + head.transform.right * 0.25f;
        lineRenderer.SetPosition(newPosition++, newPoint);

    }


    void Move()
    {
    }
}
