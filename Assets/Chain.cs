using Unity.VisualScripting;
using UnityEngine;

public class Chain : MonoBehaviour
{
    public int CaterpillarLenth = 7;
    public int headIndex = 0;
    public int tailIndex = 5;
    public bool IsHeadTurn;
    public GameObject[] spines;

    Transform headTransform, tailTransform;

    Transform[] chainLinks;
    Rigidbody2D[] spineRBs;
    DistanceJoint2D[] spineDistanceJoints;
    HingeJoint2D[] spineHingeJoints;
    [SerializeField]
    Vector3[] positions;

    LineRenderer lineRenderer;

    void Awake()
    {
        IsHeadTurn = true;
        Application.targetFrameRate = 30;

        chainLinks = new Transform[CaterpillarLenth];
        spineRBs = new Rigidbody2D[CaterpillarLenth];
        spineDistanceJoints = new DistanceJoint2D[CaterpillarLenth];
        spineHingeJoints = new HingeJoint2D[CaterpillarLenth];
        positions = new Vector3[CaterpillarLenth];
    }

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        for (int i = 0; i < CaterpillarLenth; i++)
        {
            chainLinks[i] = spines[i].GetComponent<Transform>();
            spineRBs[i] = spines[i].GetComponent<Rigidbody2D>();
            spineDistanceJoints[i] = spines[i].GetComponent<DistanceJoint2D>();
            spineHingeJoints[i] = spines[i].GetComponent<HingeJoint2D>();
        }

        headTransform = chainLinks[0].transform;
        tailTransform = chainLinks[tailIndex].transform;
    }

    void Update()
    {
        UpdateChain(1f);
        UpdateLine();
    }

    void UpdateChain(float stiffness)
    {
        Vector3 previousPosition = headTransform.position;

        for (int i = 1; i < CaterpillarLenth; i++)
        {
            Transform link = chainLinks[i];
            Vector3 direction = (previousPosition - link.position).normalized;
            link.position = previousPosition - direction * stiffness;
            previousPosition = link.position;
        }
    }

    

    void UpdateLine()
    {
        for (int i = 0; i < CaterpillarLenth; i++)
        {
            positions[i] = chainLinks[i].position;
        }
        lineRenderer.SetPositions(positions);
    }

    void TurnChange()
    {
        if (IsHeadTurn)
        {
            spineDistanceJoints[headIndex].enabled = false;
            for (int i = 1; i < CaterpillarLenth; i++)
            {
                spineDistanceJoints[i].connectedBody = spineRBs[i - 1];
            }
        }
    }
}
