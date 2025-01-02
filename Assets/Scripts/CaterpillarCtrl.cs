using JetBrains.Annotations;
using NUnit.Framework.Interfaces;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;

public enum State : int { start, wait_head, head, wait_tail, tail, end }
public class CaterpillarCtrl : MonoBehaviour
{
    public static State turn;

    public GameObject eye;

    public GameObject head, tail;
    public GameObject[] bone;
    Rigidbody2D head_rb, tail_rb;

    public FollowTarget[] followTarget = new FollowTarget[9];

    public Joystick joyStick;
    public CameraCtrl cameraCtrl;

    public GameObject DefeatPanel;
    public GameObject ClearPanel;
    Head_Tail _head, _tail;

    public float stateChangeDelay = 0.2f ;
    public float moveSpeed;
    private bool isHeadTurn;

    private bool isRunning_head;
    public bool IsRunning_head { get => isRunning_head; }
    private bool isRunning_tail;
    public bool IsRunning_tail { get => isRunning_tail; }

    private bool isHeadCoroutineRun = false;
    private bool isTailCoroutineRun = false;

    public float speed_KeyBoard = 1.0f;
    public float speed_JoyStick = 4.0f;       //  Ӹ          ̵   ӵ 

    Vector3[] testVectors = new Vector3[9];
    float error = 0.1f;
    Vector2 input;
    Vector2 antiGravityForce;
    public float rotationSpeed;

    bool isDefeat, isClear;

    Coroutine waitCoroutine;
    Coroutine turnCoroutine;
    Coroutine fixHead, fixTail;

    [SerializeField] private SoundCtrl soundCtrl;
    [SerializeField] private BestScoreManager bestScoreManager;
    void Awake()
    {
        Application.targetFrameRate = 60;
        isDefeat = false;
        isClear = false;
        rotationSpeed = 90f;

        antiGravityForce = -Physics2D.gravity;
        isRunning_head = false;
        isRunning_tail = false;
        isHeadTurn = false;
        turn = State.start;

        head_rb = head.GetComponent<Rigidbody2D>();
        tail_rb = tail.GetComponent<Rigidbody2D>();

        _head = head.GetComponent<Head_Tail>();
        _tail = tail.GetComponent<Head_Tail>();

        input = Vector2.zero;


        Bone.head = head.transform;
        Bone.tail = tail.transform;
    }

    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
    }

    void Update()
    {
        Bone.Calculate(input);
        switch (turn)
        {
            case State.start:
                {
                    tail_rb.constraints = RigidbodyConstraints2D.FreezeAll;
                    waitCoroutine = null; turnCoroutine = null; fixHead = null; fixTail = null;
                    turn = State.wait_head;
                    break;
                }
            case State.wait_head:
                {
                    tail_rb.gravityScale = 1f;
                    if (input != Vector2.zero)
                    {
                        head_rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                        head_rb.gravityScale = 0f;
                        head.transform.rotation = Quaternion.identity;
                        turn = State.head;
                    }
                    break;
                }
            case State.head:
                {
                    Move();
                    if (input == Vector2.zero)
                    {
                        if (turnCoroutine == null)
                        {
                            turnCoroutine = StartCoroutine(TurnEnd());
                            head_rb.gravityScale = 1f;
                        }
                    }
                    else
                    {
                        if (turnCoroutine != null)
                        {
                            StopCoroutine(turnCoroutine);
                            turnCoroutine = null;
                            head_rb.gravityScale = 0f;
                            head_rb.totalForce = Vector2.zero;
                        }
                    }
                    break;
                }
            case State.wait_tail:
                {
                    head_rb.gravityScale = 1f;
                    if (input != Vector2.zero)
                    {
                        tail_rb.constraints = RigidbodyConstraints2D.None;
                        tail_rb.gravityScale = 0f;
                        tail.transform.rotation = Quaternion.identity;
                        turn = State.tail;  
                    }
                    break;
                }
            case State.tail:
                {
                    Move();
                    if (input == Vector2.zero)
                    {
                        if (turnCoroutine == null)
                        {
                            turnCoroutine = StartCoroutine(TurnEnd());
                            tail_rb.gravityScale = 1f;
                        }
                    }
                    else
                    {
                        if (turnCoroutine != null)
                        {
                            StopCoroutine(turnCoroutine);
                            turnCoroutine = null;
                            tail_rb.gravityScale = 0f;
                            tail_rb.totalForce = Vector2.zero;
                        }
                    }
                    break;
                }
            case State.end:
                {
                    turn = State.wait_head;
                    break;
                }
        }
    }
    private void LateUpdate()
    {
        if (isDefeat) return;
        if (isClear) return;
        if (_head.dead || _tail.dead)
        {
            Defeat();
        }
        if (_head.clear)
        {
            Clear();
        }
    }

    float Distance(int a, int b) { return Vector3.Distance(testVectors[a],testVectors[b]); }
    void Move()
    {
        // 사용자 입력으로 이동 벡터를 계산 (예: 입력 처리 추가)
        Vector3 moveVector = input * Time.deltaTime * moveSpeed;

        if (turn == State.head || turn == State.wait_tail)
        {
            for(int i = 0; i < 9; i++)
            {
                testVectors[i] = bone[i].transform.position;
            }

            // head 이동
            testVectors[0] += moveVector / Distance(0, 1);

            // bone 간 이동 계산
            for (int i = 1; i < 6; i++)
            {
                Vector3 direction = (testVectors[i-1] - testVectors[i]).normalized;
                float distance = Distance(i, i - 1);
                float degree = (Distance(i, i + 1) > 1f + error) ? 7f : 10f;
                testVectors[i] += direction * (distance - 1f) * Time.deltaTime * degree;
            }

            // 모든 bone 간 거리가 조건에 부합하는지 확인
            for (int i = 5; i > 0; i--) //5,4,3,2,1
            {
                if (Distance(i, i + 1) > 1f + error)
                {
                    testVectors[i] += (testVectors[i + 1] - testVectors[i]) * Time.deltaTime;
                }
            }

            // 모든 bone 위치 업데이트
            for (int i = 0; i < 6; i++)
            {
                bone[i].transform.position = testVectors[i];
            }

            // 이동 벡터가 0이 아닌 경우에만 회전 및 이동 수행
            if (moveVector != Vector3.zero)
            {
                // 목표 각도 계산
                float angle = Mathf.Atan2(moveVector.y, moveVector.x) * Mathf.Rad2Deg;

                // 현재 각도에서 목표 각도로 부드럽게 회전
                head.transform.rotation = Quaternion.Euler(0, 0, angle); // Z축 회전 적용
            }
        }

        else if(turn == State.tail || turn == State.wait_head)
        {
            Vector3 directionVector = (head.transform.position - tail.transform.position);
            Vector3 normalVector = new Vector3(-directionVector.y, directionVector.x, 0).normalized;
            float dist = directionVector.magnitude;

            for (int i = 0; i < 9; i++)
            {
                testVectors[i] = bone[i].transform.position;
            }

            // tail 이동
            testVectors[6] += moveVector / Distance(6, 5);

            // bone 간 이동 계산
            for (int i = 5; i > 0; i--)
            {
                Vector3 direction = (testVectors[i + 1] - testVectors[i]).normalized;
                float distance = Distance(i, i + 1);
                float degree = (Distance(i, i - 1) > 1f + error) ? 5f : 10f;
                testVectors[i] += direction * (distance - 1f) * Time.deltaTime * degree;
            }

            // 모든 bone 간 거리가 조건에 부합하는지 확인
            for (int i = 1; i < 6; i++) //1,2,3,4,5
            {
                if (Distance(i, i - 1) > 1f + error)
                {
                    testVectors[i] += (testVectors[i - 1] - testVectors[i]) * Time.deltaTime * 2;
                }
                else
                {
                    if (Distance(i,i+1) < 1f + error && Distance(i, i - 1) < 1f + error && dist < 5.5f && dist > 1.5f)
                        testVectors[i] += normalVector * Time.deltaTime * Mathf.Sin(Mathf.PI * i / 6) * 5f / dist;
                }
            }

            // 모든 bone 위치 업데이트
            for (int i = 1; i < 9; i++)
            {
                bone[i].transform.position = testVectors[i];
            }
        }

        for(int i = 7; i < 9; i++)
        {
            Vector3 direction = (testVectors[i - 1] - testVectors[i]).normalized;
            float distance = Distance(i, i - 1);
            testVectors[i] += direction * (distance - 1f) * Time.deltaTime * 10f;
        }
        bone[7].transform.position = testVectors[7];
        bone[8].transform.position = testVectors[8];
    }

    IEnumerator TurnEnd()
    {
        yield return new WaitForSeconds(stateChangeDelay);
        if (turn == State.head)
        {
            if(fixHead == null) fixHead = StartCoroutine(FixHead());
        }
        else
        {
            if (fixTail == null) fixTail = StartCoroutine(FixTail());
        }
        turn++;
        cameraCtrl.MoveCamera();
    }

    IEnumerator FixHead()
    {
        _head = head.GetComponent<Head_Tail>();
        while (!_head.isAttach)
        {
            head.transform.rotation = Quaternion.identity;
            yield return null;
        }
        head_rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    IEnumerator FixTail()
    {
        _tail = tail.GetComponent<Head_Tail>();
        while (!_tail.isAttach)
        {
            tail.transform.rotation = Quaternion.identity;
            yield return null;
        }
        tail_rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void TurnStart()
    {
        if (isHeadTurn)
        {
            turn = State.head;
        }
        else turn = State.tail;
    }

    public void Defeat()
    {
        isDefeat = true;
        soundCtrl.StartDefeatSound();
        bestScoreManager.SetBestScore();
        DefeatPanel.SetActive(true);
    }

    public void Clear()
    {
        isClear = true;
        soundCtrl.StartClearSound();
        bestScoreManager.SetBestScore(true);
        ClearPanel.SetActive(true);
    }
}
