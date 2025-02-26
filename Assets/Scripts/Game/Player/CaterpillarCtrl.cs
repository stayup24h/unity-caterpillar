using eventChannel;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum State : int { start, wait_head, head, wait_tail, tail, end }
public class CaterpillarCtrl : MonoBehaviour
{
    public static State turn;
    public static float distance;
    public GameObject eye;
    public FloatingJoystick floatingJoystick;
    public GameObject JoystickObject;

    public GameObject head, tail;
    public GameObject[] bone;
    Rigidbody2D head_rb, tail_rb;
    Rigidbody2D[] bone_rb;
    DistanceJoint2D head_dj, tail_dj;
    DistanceJoint2D[] bone_dj;

    public FollowTarget[] followTarget = new FollowTarget[7];

    public Joystick joyStick;
    public CameraCtrl cameraCtrl;

    public GameObject DefeatPanel;
    public GameObject RemixDefeatPanel;
    public GameObject ClearPanel;
    Head_Tail _head, _tail;

    public float stateChangeDelay = 0.2f;
    public float moveSpeed;

    private bool isRunning_head;
    public bool IsRunning_head { get => isRunning_head; }
    private bool isRunning_tail;
    public bool IsRunning_tail { get => isRunning_tail; }

    public float speed_KeyBoard = 1.0f;
    public float speed_JoyStick = 4.0f;       //  Ӹ          ̵   ӵ 

    Vector3[] testVectors = new Vector3[9];
    Vector3[] moveTo = new Vector3[9];
    Vector2 input;
    public bool onCtrl { get { return input != Vector2.zero; } private set {; } }
    public float rotationSpeed;

    bool isDefeat, isClear;

    Coroutine waitCoroutine;
    Coroutine turnCoroutine;
    public Coroutine fixHead, fixTail;
    bool isFixHead, isFixTail;

    [SerializeField] private SoundCtrl soundCtrl;

    [Header("EventChannel")]
    [SerializeField] private EventChannelSO clearEventChannel;
    [SerializeField] private EventChannelSO endEventChannel;

    void Awake()
    {
        JoystickObject.SetActive(true);
        floatingJoystick.Initialize(this);
        Application.targetFrameRate = 60;
        isDefeat = false;
        isClear = false;
        isFixHead = false;
        isFixTail = true;
        rotationSpeed = 90f;

        isRunning_head = false;
        isRunning_tail = false;
        turn = State.start;

        head_rb = head.GetComponent<Rigidbody2D>();
        tail_rb = tail.GetComponent<Rigidbody2D>();

        head_dj = head.GetComponents<DistanceJoint2D>()[1];
        tail_dj = tail.GetComponents<DistanceJoint2D>()[1];

        _head = head.GetComponent<Head_Tail>();
        _tail = tail.GetComponent<Head_Tail>();

        bone_rb = new Rigidbody2D[bone.Length];
        bone_dj = new DistanceJoint2D[bone.Length];
        for (int i = 0; i < bone.Length; i++)
        {
            bone_rb[i] = bone[i].GetComponent<Rigidbody2D>();
        }
        for (int i = 0; i < 7; i++)
        {
            bone_dj[i] = bone[i].GetComponent<DistanceJoint2D>();
        }

        head_dj = bone_dj[0]; tail_dj = bone_dj[6];
        input = Vector2.zero;


        Bone.head = head.transform;
        Bone.tail = tail.transform;
    }

    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
    }

    public void OnJoystickMove()
    {
        input = new Vector2(floatingJoystick.Horizontal, floatingJoystick.Vertical);
    }

    void Update()
    {
        Bone.Calculate(input);
        distance = Distance(0, 6);
        // 현재 위치 받아오기
        for (int i = 0; i < 9; i++)
        {
            testVectors[i] = bone[i].transform.position;
        }
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
                        isFixHead = false;
                        head.transform.rotation = Quaternion.identity;
                        turn = State.head;

                        for (int i = 1; i < 6; i++)
                        {
                            bone_dj[i].connectedBody = bone[i + 1].GetComponent<Rigidbody2D>();
                            bone_rb[i].gravityScale = 0f;
                        }
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
                            for(int i = 1; i < 6; i++)
                            {
                                bone_rb[i].totalForce = Vector2.zero;
                            }
                        }
                    }
                    break;
                }
            case State.wait_tail:
                {
                    head_rb.gravityScale = 1f;
                    
                    if (input != Vector2.zero)
                    {
                        isFixTail = false;
                        tail.transform.rotation = Quaternion.identity;
                        turn = State.tail;

                        for (int i = 1; i < 6; i++)
                        {
                            bone_dj[i].connectedBody = bone[i - 1].GetComponent<Rigidbody2D>();
                            bone_rb[i].gravityScale = 0.1f;
                        }
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
                            for (int i = 1; i < 6; i++)
                            {
                                bone_rb[i].totalForce = Vector2.zero;
                            }
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
        if(Distance(0,6)>6 && isFixTail && isFixHead)
        {
            Debug.Log("asdf");
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

    float Distance(int a, int b) { return Vector3.Distance(testVectors[a], testVectors[b]); }
    void Move()
    {
        // 사용자 입력으로 이동 벡터를 계산
        Vector3 moveVector = input * Time.deltaTime * moveSpeed;

     

        if (turn == State.head) testVectors[0] += moveVector;
        if (turn == State.tail) testVectors[6] += moveVector;


        for (int i = 1; i < 6; i++)
        {
            moveTo[i] = moveVector;
            if (Distance(i - 1, i) > 1f)
            {
                moveTo[i] += (testVectors[i - 1] - testVectors[i]).normalized * Time.deltaTime;
            }
            if (Distance(i, i + 1) > 1f)
            {
                moveTo[i] += (testVectors[i + 1] - testVectors[i]).normalized * Time.deltaTime;
            }
            if (Distance(0, i) > i)
            {
                moveTo[i] += (testVectors[0] - testVectors[i]).normalized * Time.deltaTime;
            }
            if (Distance(i, 6) > 6 - i)
            {
                moveTo[i] += (testVectors[6] - testVectors[i]).normalized * Time.deltaTime;
            }
        }


        if (turn == State.head && moveVector != Vector3.zero)
        {
            // 목표 각도 계산
            float angle = Mathf.Atan2(moveVector.y, moveVector.x) * Mathf.Rad2Deg;

            // 현재 각도에서 목표 각도로 부드럽게 회전
            head.transform.rotation = Quaternion.Euler(0, 0, angle); // Z축 회전 적용

            //head_dj.enable = false;
        }
        else if (turn == State.head && moveVector == Vector3.zero)
        {
            //head_dj.enable.true;
        }

        else if (turn == State.tail)
        {
            Vector3 directionVector = (head.transform.position - tail.transform.position);
            Vector3 normalVector = new Vector3(-directionVector.y, directionVector.x, 0).normalized;
            float dist = directionVector.magnitude;

            // bone 간 이동 계산
            for (int i = 5; i > 0; i--)
            {
                Vector3 direction = (testVectors[i + 1] - testVectors[i]).normalized;
                float distance = Distance(i, i + 1);
                float degree = (Distance(i, i - 1) > 1f) ? 5f : 10f;
                testVectors[i] += direction * (distance - 1f) * Time.deltaTime * degree;
            }

            // 모든 bone 간 거리가 조건에 부합하는지 확인
            for (int i = 1; i < 6; i++) //1,2,3,4,5
            {
                if (Distance(i, i - 1) > 1f)
                {
                    testVectors[i] += (testVectors[i - 1] - testVectors[i]) * Time.deltaTime * 2;
                }
                else
                {
                    if (Distance(i, i + 1) < 1f && dist < 5.5f)
                    {
                        testVectors[i] += normalVector * Time.deltaTime * Mathf.Sin(Mathf.PI * i / 6) * 5f / dist * 2f;
                        if (Distance(i, i + 1) > 1f && Distance(i, i - 1) > 1f)
                            testVectors[i] -= normalVector * Time.deltaTime * Mathf.Sin(Mathf.PI * i / 6) * 5f / dist;
                    }
                }
            }


        }

        // 모든 bone 위치 업데이트
        for (int i = 0; i < 9; i++)
        {
            bone[i].transform.position = testVectors[i];
        }

        for (int i = 7; i < 9; i++)
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
            if (fixHead == null) fixHead = StartCoroutine(FixHead());
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
        for (int i = 1; i < 6; i++)
        {
            bone_rb[i].gravityScale = 0.1f;
        }
    }

    IEnumerator FixTail()
    {
        _tail = tail.GetComponent<Head_Tail>();
        while (!_tail.isAttach)
        {
            tail.transform.rotation = Quaternion.identity;
            yield return null;
        }
        for (int i = 1; i < 6; i++)
        {
            bone_rb[i].gravityScale = 0.1f;
        }
    }

    public void Defeat()
    {
        isDefeat = true;
        soundCtrl.StartDefeatSound();
        endEventChannel.RaiseEvent();
        JoystickObject.SetActive(false);
        if (GameManager.Instance.MapType == MapType.stage)
        {
            DefeatPanel.SetActive(true);
        }
        else if (GameManager.Instance.MapType == MapType.remix)
        {
            RemixDefeatPanel.SetActive(true);
        }
    }

    public void Clear()
    {
        isClear = true;
        soundCtrl.StartClearSound();
        clearEventChannel.RaiseEvent();
        JoystickObject.SetActive(false);
        ClearPanel.SetActive(true);
    }
}
