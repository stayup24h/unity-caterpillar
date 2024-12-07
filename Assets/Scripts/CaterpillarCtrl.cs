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

    public GameObject DefeatPanel;
    public GameObject ClearPanel;

    Head_Tail _head, _tail;

    public State state;
    private bool isHeadTurn;   //  Ӹ                     ư    ó  

    private bool isRunning_head;
    public bool IsRunning_head { get => isRunning_head; }
    private bool isRunning_tail;
    public bool IsRunning_tail { get => isRunning_tail; }

    private bool isHeadCoroutineRun = false;
    private bool isTailCoroutineRun = false;

    public float speed_KeyBoard = 1.0f;
    public float speed_JoyStick = 4.0f;       //  Ӹ          ̵   ӵ 

    Vector2 antiGravityForce;
    public float rotationSpeed;

    bool isDefeat, isClear;

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
        state = State.none;

        for (int i = 0; i < bone.Length; i++)
        {
            bone_rb[i] = bone[i].GetComponent<Rigidbody2D>();
        }
        head = bone[0]; tail = bone[6];
        head_rb = bone_rb[0]; tail_rb = bone_rb[6];

        tail_rb.constraints = RigidbodyConstraints2D.FreezeAll;

        _head = head.GetComponent<Head_Tail>();
        _tail = tail.GetComponent<Head_Tail>();
    }

    void FixedUpdate()
    {
        if (state == State.none)
        {
            if (!isHeadCoroutineRun) StartCoroutine(FixHead());
            if (!isTailCoroutineRun) StartCoroutine(FixTail());
            state = State.wait;
            cameraCtrl.MoveCamera(isHeadTurn);
        }
        else if (state == State.wait)
        {
            if (isHeadCoroutineRun && isTailCoroutineRun) cameraCtrl.MoveCameraToMidPos();
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
        isHeadCoroutineRun = true;
        _head = head.GetComponent<Head_Tail>();
        while (!_head.isAttach)
        {
            head.transform.rotation = Quaternion.identity;
            yield return null;
        }
        head_rb.constraints = RigidbodyConstraints2D.FreezeAll;

        isHeadCoroutineRun = false;
        isRunning_head = false;
    }

    IEnumerator FixTail()
    {
        isTailCoroutineRun = true;
        _tail = tail.GetComponent<Head_Tail>();
        while (!_tail.isAttach)
        {
            tail.transform.rotation = Quaternion.identity;
            yield return null;
        }

        tail_rb.constraints = RigidbodyConstraints2D.FreezeAll;

        isTailCoroutineRun = false;
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
        soundCtrl.StartMoveSound();
    }

    void MoveHead_KeyBoard()
    {
        isRunning_head = true;

        head_rb.constraints = RigidbodyConstraints2D.None;
        head.transform.rotation = Quaternion.identity;

        Vector3 move = inputCtrl.GetMove();

        //         
        if (move != Vector3.zero)
        {
            //     ȭ                    
            float targetAngle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg;

            //      ȸ              ǥ        õõ   ȸ  
            float angle = Mathf.MoveTowardsAngle(eye.transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);

            //       Ʈ   ȸ  
            eye.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        // Ű        ⿡       Ӹ     ̵ 
        move = move * speed_KeyBoard * Time.deltaTime;
        head.transform.Translate(move);
        head_rb.AddForce(antiGravityForce); // ߷     
    }

    void MoveTail_KeyBoard()
    {
        isRunning_tail = true;

        tail_rb.constraints = RigidbodyConstraints2D.None;
        tail.transform.rotation = Quaternion.identity;

        // Ű        ⿡              ̵ 
        Vector3 move = inputCtrl.GetMove() * speed_KeyBoard * Time.deltaTime;
        tail.transform.Translate(move);
        tail_rb.AddForce(antiGravityForce); // ߷     
    }
    void MoveHead_JoyStick()
    {
        head_rb.constraints = RigidbodyConstraints2D.None;
        head.transform.rotation = Quaternion.identity;

        //    ̽ ƽ    ⿡       Ӹ     ̵ 
        Vector3 move = new Vector3(joyStick.Direction.x, joyStick.Direction.y, 0) * speed_JoyStick * Time.deltaTime;
        head.transform.Translate(move);
        head_rb.AddForce(antiGravityForce); // ߷     
    }

    void MoveTail_JoyStick()
    {
        tail_rb.constraints = RigidbodyConstraints2D.None;
        tail.transform.rotation = Quaternion.identity;

        //    ̽ ƽ    ⿡              ̵ 
        Vector3 move = new Vector3(joyStick.Direction.x, joyStick.Direction.y, 0) * speed_JoyStick * Time.deltaTime;
        tail.transform.Translate(move);
        tail_rb.AddForce(antiGravityForce); // ߷     
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
