using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class CaterpillarCtrl : MonoBehaviour
{
    public enum State { none, wait, head, tail }

    public GameObject[] bone = new GameObject[9];
    private Rigidbody2D[] bone_rb = new Rigidbody2D[9];

    private GameObject head, tail;
    private Rigidbody2D head_rb, tail_rb;

    public Joystick joyStick;

    static public State state;
    static private bool isHeadTurn;   // 머리와 꼬리 순서를 번갈아가며 처리

    bool isRunning_head;
    bool isRunning_tail;
    private float speed = 4.0f;       // 머리와 꼬리 이동 속도

    Vector2 antiGravityForce;

    void Start()
    {
        antiGravityForce = -Physics2D.gravity;
        isRunning_head = false;
        isRunning_tail = false;
        isHeadTurn = false;
        state = State.none;

        for (int i = 0; i < bone.Length; i++)
        {
            bone_rb[i] = bone[i].GetComponent<Rigidbody2D>();
        }
        head = bone[0]; tail = bone[5];
        head_rb = bone_rb[0]; tail_rb = bone_rb[5];
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
            MoveHead();
        }
        else
        {
            MoveTail();
        }
    }

    IEnumerator FixHead()
    {
        isRunning_head = true;
        Head_Tail _head = head.GetComponent<Head_Tail>();
        while (true)
        {
            if (_head.isAttach) break;
            yield return null;
        }

        Debug.Log("헤드 고정");
        head_rb.constraints = RigidbodyConstraints2D.FreezeAll;
        isRunning_head = false;
    }

    IEnumerator FixTail()
    {
        isRunning_tail = true;
        Head_Tail _tail = tail.GetComponent<Head_Tail>();
        while (!_tail.isAttach)
        {
            yield return new WaitForFixedUpdate();
        }
        Debug.Log("꼬리 고정");
        tail_rb.constraints = RigidbodyConstraints2D.FreezeAll;
        isRunning_tail = false;
    }

    static public void TurnStart()
    {
        if (isHeadTurn)
        {
            state = State.head;
        }
        else state = State.tail;
    }

    static public void TurnEnd()
    {
        state = State.none;
        isHeadTurn = !isHeadTurn;
    }

    void MoveHead()
    {
        head_rb.constraints = RigidbodyConstraints2D.None;
        head.transform.rotation = Quaternion.identity;

        // 조이스틱 방향에 따라 머리를 이동
        Vector3 move = new Vector3(joyStick.Direction.x, joyStick.Direction.y, 0) * speed * Time.deltaTime;
        head.transform.Translate(move);
        head_rb.AddForce(antiGravityForce); //중력 상쇄
    }

    void MoveTail()
    {
        tail_rb.constraints = RigidbodyConstraints2D.None;
        tail.transform.rotation = Quaternion.identity;
        // 조이스틱 방향에 따라 꼬리를 이동
        Vector3 move = new Vector3(joyStick.Direction.x, joyStick.Direction.y, 0) * speed * Time.deltaTime;
        tail.transform.Translate(move);
        tail_rb.AddForce(antiGravityForce); //중력 상쇄
    }
}
