using JetBrains.Annotations;
using System.Collections;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public enum TurnEnum : int { waitHead, head, waitTail, tail, end }
public class Caterpillar : MonoBehaviour
{
    public static TurnEnum turnState;

    public GameObject[] spines;
    Spine[] spineClass;

    public GameObject head, tail;
    Rigidbody2D headRB, tailRB;
    int tailNum = 6;

    Vector2 input;
    GameObject moveTarget;
    Rigidbody2D moveTargetRB;

    LineRenderer lineRenderer;

    Chain[] bodyChains = new Chain[9];

    Coroutine waitCoroutine;

    void Awake()
    {
        Application.targetFrameRate = 60;
        AddDistanceJoint();
        InitializeLineRenderer();

        spineClass = new Spine[spines.Length];
        for(int i = 0; i < spines.Length; i++)
            spineClass[i] = spines[i].GetComponent<Spine>();

        head = spines[0]; tail = spines[tailNum];
        headRB = head.GetComponent<Rigidbody2D>();
        tailRB = tail.GetComponent<Rigidbody2D>();
        turnState = TurnEnum.waitHead;
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

    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();

    }

    void Update()
    {
        switch (turnState)
        {
            case TurnEnum.waitHead:
                {
                    if (input != Vector2.zero)
                    {
                        for (int i = 0; i < spines.Length; i++)
                        {
                            if (i == 0) spineClass[i].TurnChange_Head(turnState);
                            else if (i == tailNum) spineClass[i].TurnChange_Tail(turnState);
                            else spineClass[i].TurnChange(turnState);
                        }
                        moveTarget = head;
                        moveTargetRB = headRB;
                        turnState = TurnEnum.head;
                    }
                    break;
                }
            case TurnEnum.head:
                {
                    if(input == Vector2.zero)
                    {
                        if (waitCoroutine == null)
                        {
                            waitCoroutine = StartCoroutine(Wait());
                        }
                    }
                    else
                    {
                        if (waitCoroutine != null)
                        {
                            StopCoroutine(waitCoroutine);
                            waitCoroutine = null;
                        }
                    }
                    break;
                }
            case TurnEnum.waitTail:
                {
                    if (input != Vector2.zero)
                    {
                        for (int i = 0; i < spines.Length; i++)
                        {
                            if (i == 0) spineClass[i].TurnChange_Head(turnState);
                            else if (i == tailNum) spineClass[i].TurnChange_Tail(turnState);
                            else spineClass[i].TurnChange(turnState);
                        }
                        moveTarget = tail;
                        moveTargetRB = tailRB;
                        turnState = TurnEnum.tail;
                    }
                    break;
                }
            case TurnEnum.tail:
                {
                    if (input == Vector2.zero)
                    {
                        if (waitCoroutine == null)
                        {
                            waitCoroutine = StartCoroutine(Wait());
                        }
                            
                    }
                    else
                    {
                        if (waitCoroutine != null)
                        {
                            StopCoroutine(waitCoroutine);
                            waitCoroutine = null;
                        }
                            
                    }
                    break;
                }
            case TurnEnum.end:
                {
                    turnState = TurnEnum.waitHead;
                    break;
                }
        }
        
    }

    void Move()
    {
        Vector2 targetVelocity = Vector2.zero;

        // 입력이 있는 경우 이동 속도 설정
        if (input != Vector2.zero)
        {
            targetVelocity = new Vector2(input.x, input.y) * 1f;
            moveTargetRB.linearVelocity = targetVelocity; // linearVelocity 대신 velocity 사용 (Rigidbody2D에서)
        }

        // 입력에 따라 즉시 회전
        if (turnState == TurnEnum.head && input != Vector2.zero)
        {
            float angle = Mathf.Atan2(targetVelocity.y, targetVelocity.x) * Mathf.Rad2Deg;
            moveTarget.transform.rotation = Quaternion.Euler(0, 0, angle); // 즉시 회전
        }
    }




    IEnumerator Wait()
    {
        Debug.Log("Waiting for 1 second...");
        yield return new WaitForSeconds(1f);

        turnState++;
        Debug.Log($"Turn changed to: {turnState}");

        waitCoroutine = null;
    }



    void LateUpdate()
    {
        UpdateLineRenderer();
        if (turnState == TurnEnum.head || turnState == TurnEnum.tail) Move(); 
    }

    void InitializeLineRenderer()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = spines.Length * 2 + 3;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
    }

    void UpdateLineRenderer()
    {
        int newPosition = 0;
        Vector3 newPoint;


        newPoint = head.transform.position + head.transform.right * 0.25f;
        lineRenderer.SetPosition(newPosition++, newPoint);

        for (int i = 0; i < spines.Length; i++)
        {
            if (spines[i] != null)
            {
                newPoint = spines[i].transform.position - spines[i].transform.up * 0.25f;
                lineRenderer.SetPosition(newPosition++, newPoint);
            }
        }

        newPoint = spines[spines.Length - 1].transform.position - spines[spines.Length - 1].transform.right * 0.25f;
        lineRenderer.SetPosition(newPosition++, newPoint);

        for (int i = spines.Length - 1; i >= 0; i--)
        {
            if (spines[i] != null)
            {
                newPoint = spines[i].transform.position + spines[i].transform.up * 0.25f;
                lineRenderer.SetPosition(newPosition++, newPoint);
            }
        }

        newPoint = head.transform.position + head.transform.right * 0.25f;
        lineRenderer.SetPosition(newPosition++, newPoint);

    }
}
