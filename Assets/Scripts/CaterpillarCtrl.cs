using JetBrains.Annotations;
using NUnit.Framework.Interfaces;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public enum State : int { start, wait_head, head, wait_tail, tail, end }
public class CaterpillarCtrl : MonoBehaviour
{
    public static State turn;
    
    public GameObject eye;

    public GameObject head, tail;
    GameObject target;
    Rigidbody2D head_rb, tail_rb;
    Rigidbody2D target_rb;

    public Joystick joyStick;
    public CameraCtrl cameraCtrl;

    public GameObject DefeatPanel;
    public GameObject ClearPanel;
    Head_Tail _head, _tail;
    
    
    private bool isHeadTurn;

    private bool isRunning_head;
    public bool IsRunning_head { get => isRunning_head; }
    private bool isRunning_tail;
    public bool IsRunning_tail { get => isRunning_tail; }

    private bool isHeadCoroutineRun = false;
    private bool isTailCoroutineRun = false;

    public float speed_KeyBoard = 1.0f;
    public float speed_JoyStick = 4.0f;       //  Ӹ          ̵   ӵ 

    Vector2 input;
    Vector2 antiGravityForce;
    public float rotationSpeed;

    bool isDefeat, isClear;

    Coroutine waitCoroutine;
    Coroutine fixHead, fixTail;

    [SerializeField] private SoundCtrl soundCtrl;
    [SerializeField] private BestScoreManager bestScoreManager;
    void Awake()
    {
        Application.targetFrameRate = 30;
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
                    waitCoroutine = null; fixHead = null; fixTail = null;
                    turn = State.wait_head;
                    break;
                }
            case State.wait_head:
                {
                    tail_rb.gravityScale = 1f;
                    if (input != Vector2.zero)
                    {
                        head_rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                        head.transform.rotation = Quaternion.identity;          
                        turn = State.head;
                        head_rb.gravityScale = 0f;

                        target = head;
                        target_rb = head_rb;
                    }
                    break;
                }
            case State.head:
                {
                    Move();
                    if (input == Vector2.zero)
                    {
                        if (waitCoroutine == null)
                        {
                            waitCoroutine = StartCoroutine(Wait());
                            head_rb.gravityScale = 1f;
                        }
                            
                    }
                    else
                    {
                        if (waitCoroutine != null)
                        {
                            StopCoroutine(waitCoroutine);
                            head_rb.gravityScale = 0f;
                            head_rb.totalForce = Vector2.zero;
                            
                            waitCoroutine = null;
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
                        tail.transform.rotation = Quaternion.identity;
                        target = tail;
                        target_rb = tail_rb;
                        turn = State.tail;
                        target_rb.gravityScale = 0f;
                    }
                    break;
                }
            case State.tail:
                {
                    Move();
                    if (input == Vector2.zero)
                    {
                        if (waitCoroutine == null)
                            waitCoroutine = StartCoroutine(Wait());
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
            case State.end:
                {
                    turn = State.wait_head;
                    break;
                }
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);

        if (turn == State.head) fixHead = StartCoroutine(FixHead());
        else if (turn == State.tail) fixTail = StartCoroutine(FixTail());
        turn++;

        waitCoroutine = null;
    }

    void Move()
    {
        // 사용자 입력으로부터 이동 방향을 계산 (예: 입력 처리 추가)
        Vector2 move = input;
        
        if (Vector2.Distance(head.transform.position, tail.transform.position) >= 6f)
        {
            Vector2 direction = (head.transform.position - tail.transform.position).normalized;
            move *= 0.2f;
            //if (Vector2.Distance(move, direction) < 1f) return;
        }

        // 이동 벡터가 0이 아닌 경우에만 회전 및 이동 수행
        if (move != Vector2.zero)
        {
            // 목표 각도 계산
            float angle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg;

            // 현재 각도에서 목표 각도로 부드럽게 회전
            head.transform.rotation = Quaternion.Euler(0, 0, angle); // Z축 회전 적용

            // 타겟 오브젝트 회전 적용
            target.transform.rotation = Quaternion.Euler(0, 0, angle);

            // 이동 거리 계산 및 이동 수행 (월드 좌표계 기준으로)
            Vector2 moveDelta = move.normalized * speed_KeyBoard * Time.deltaTime;
            target.transform.Translate(new Vector3(moveDelta.x, moveDelta.y, 0), Space.World);
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

    IEnumerator FixHead()
    {
        _head = head.GetComponent<Head_Tail>();
        while (!_head.isAttach)
        {
            head.transform.rotation = Quaternion.identity;
            yield return null;
        }
        head_rb.constraints = RigidbodyConstraints2D.FreezeAll;
        fixHead = null;
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
        fixTail = null;
    }

    public void TurnStart()
    {
        if (isHeadTurn)
        {
            turn = State.head;
        }
        else turn = State.tail;
    }

    void MoveHead_JoyStick()
    {
        head_rb.constraints = RigidbodyConstraints2D.None;
        head.transform.rotation = Quaternion.identity;

        Vector3 move = new Vector3(joyStick.Direction.x, joyStick.Direction.y, 0) * speed_JoyStick * Time.deltaTime;
        head.transform.Translate(move);
        head_rb.AddForce(antiGravityForce);   
    }

    void MoveTail_JoyStick()
    {
        tail_rb.constraints = RigidbodyConstraints2D.None;
        tail.transform.rotation = Quaternion.identity;

        Vector3 move = new Vector3(joyStick.Direction.x, joyStick.Direction.y, 0) * speed_JoyStick * Time.deltaTime;
        tail.transform.Translate(move);
        tail_rb.AddForce(antiGravityForce); 
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
