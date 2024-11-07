using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class CaterpillarCtrl : MonoBehaviour
{
    public enum State { none, wait, head, tail }

    public GameObject[] bone = new GameObject[9];
    private Rigidbody2D[] bone_rb = new Rigidbody2D[9];
    public GameObject eye;

    private GameObject head, tail;
    private Rigidbody2D head_rb, tail_rb;

    public InputCtrl inputCtrl;
    public Joystick joyStick;
    public CameraCtrl cameraCtrl;

    public State state;
    private bool isHeadTurn;   // 머리와 꼬리 순서를 번갈아가며 처리

    bool isRunning_head;
    bool isRunning_tail;

    public float speed_KeyBoard = 1.0f;
    public float speed_JoyStick = 4.0f;       // 머리와 꼬리 이동 속도

    Vector2 antiGravityForce;
    public float rotationSpeed;

    void Start()
    {
        rotationSpeed = 90f;

        antiGravityForce = -Physics2D.gravity;
        isRunning_head = false;
        isRunning_tail = false;
        isHeadTurn = false;
        state = State.none;

        for (int i = 0; i < bone.Length; i++)
        {
            bone_rb[i] = bone[i].GetComponent<Rigidbody2D>();
        }
        head = bone[0]; tail = bone[6];
        head_rb = bone_rb[0]; tail_rb = bone_rb[6];

        tail_rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    void FixedUpdate()
    {
        if (state == State.none)
        {
            if (!isRunning_head) StartCoroutine(FixHead());
            if (!isRunning_tail) StartCoroutine(FixTail());
            state = State.wait;
        }
        else if (state == State.wait)
        {
            ;
        }
        else if (state == State.head)
        {
            //MoveHead_JoyStick();
            MoveHead_KeyBoard();
        }
        else
        {
            //MoveTail_JoyStick();
            MoveTail_KeyBoard();
        }
    }

    IEnumerator FixHead()
    {
        isRunning_head = true;
        Head_Tail _head = head.GetComponent<Head_Tail>();
        while (!_head.isAttach)
        {
            head.transform.rotation = Quaternion.identity;
            yield return null;
        }
        head_rb.constraints = RigidbodyConstraints2D.FreezeAll;
        isRunning_head = false;
    }

    IEnumerator FixTail()
    {
        isRunning_tail = true;
        bool flag = isHeadTurn;
        Head_Tail _tail = tail.GetComponent<Head_Tail>();
        while (!_tail.isAttach)
        {
            tail.transform.rotation = Quaternion.identity;
            yield return null;
        }

        if(flag) cameraCtrl.Start();
        tail_rb.constraints = RigidbodyConstraints2D.FreezeAll;
        isRunning_tail = false;
    }

    public void TurnStart()
    {
        if (isHeadTurn)
        {
            state = State.head;
        }
        else state = State.tail;
    }

    public void TurnEnd()
    {
        state = State.none;
        isHeadTurn = !isHeadTurn;
    }

    void MoveHead_KeyBoard()
    {
        head_rb.constraints = RigidbodyConstraints2D.None;
        head.transform.rotation = Quaternion.identity;

        Vector3 move = inputCtrl.GetMove();

        //눈 움직임
        if (move != Vector3.zero)
        {
            // 정규화된 벡터의 각도를 계산
            float targetAngle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg;

            // 현재 회전 각도에서 목표 각도로 천천히 회전
            float angle = Mathf.MoveTowardsAngle(eye.transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);

            // 오브젝트를 회전
            eye.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        // 키보드 방향에 따라 머리를 이동
        move = move * speed_KeyBoard * Time.deltaTime;
        head.transform.Translate(move);
        head_rb.AddForce(antiGravityForce); //중력 상쇄
    }

    void MoveTail_KeyBoard()
    {
        tail_rb.constraints = RigidbodyConstraints2D.None;
        tail.transform.rotation = Quaternion.identity;

        // 키보드 방향에 따라 꼬리를 이동
        Vector3 move = inputCtrl.GetMove() * speed_KeyBoard * Time.deltaTime;
        tail.transform.Translate(move);
        tail_rb.AddForce(antiGravityForce); //중력 상쇄
    }
    void MoveHead_JoyStick()
    {
        head_rb.constraints = RigidbodyConstraints2D.None;
        head.transform.rotation = Quaternion.identity;

        // 조이스틱 방향에 따라 머리를 이동
        Vector3 move = new Vector3(joyStick.Direction.x, joyStick.Direction.y, 0) * speed_JoyStick * Time.deltaTime;
        head.transform.Translate(move);
        head_rb.AddForce(antiGravityForce); //중력 상쇄
    }

    void MoveTail_JoyStick()
    {
        tail_rb.constraints = RigidbodyConstraints2D.None;
        tail.transform.rotation = Quaternion.identity;

        // 조이스틱 방향에 따라 꼬리를 이동
        Vector3 move = new Vector3(joyStick.Direction.x, joyStick.Direction.y, 0) * speed_JoyStick * Time.deltaTime;
        tail.transform.Translate(move);
        tail_rb.AddForce(antiGravityForce); //중력 상쇄
    }
}
