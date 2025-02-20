using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;

public class Head_Tail : MonoBehaviour
{
    public CaterpillarCtrl caterpillarCtrl;
    public bool dead;
    public bool clear;
    public bool isAttach = false;
    public GameObject attachedObject;

    public bool head;
    Rigidbody2D rb;

    void Awake()
    {
        dead = false;
        rb = GetComponent<Rigidbody2D>();
        if (!head)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            isAttach = true;
            attachedObject = collision.gameObject;

        }
        if (collision.gameObject.CompareTag("Defeat")) { dead = true; }
        if (collision.gameObject.CompareTag("Clear")) { clear = true; }
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            isAttach = false;
            attachedObject = null;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            isAttach = true;
            attachedObject = collision.gameObject;
        }
    }

    private void Update()
    {
        if (head)
        {
            if(CaterpillarCtrl.turn != State.head)
            {
                rb.gravityScale = 1f;

                //붙어있고 거리짧으면 그대로
                if (isAttach && CaterpillarCtrl.distance < 7f) 
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;

                //붙어있는데 거리가 멀어질때
                if(isAttach && CaterpillarCtrl.distance > 7f)
                {
                    if (CaterpillarCtrl.turn == State.wait_tail) //꼬리턴일땐 헤드를 뗀다
                        rb.constraints = RigidbodyConstraints2D.None;
                }
                
                
                if(!isAttach) rb.constraints = RigidbodyConstraints2D.None;
            }
            else
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                if (caterpillarCtrl.onCtrl) 
                {
                    rb.gravityScale = 0f;
                    rb.totalForce = Vector2.zero;
                }
                else rb.gravityScale = 1f;
                   
            }
        }
        else
        {
            if(CaterpillarCtrl.turn != State.tail)
            {
                rb.gravityScale = 1f;

                //붙어있고 거리짧으면 그대로
                if (isAttach && CaterpillarCtrl.distance < 7f)
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;

                //붙어있는데 거리가 멀어질때
                if (isAttach && CaterpillarCtrl.distance > 7f)
                {
                    if (CaterpillarCtrl.turn == State.wait_head) //헤드턴일땐 꼬리를 뗀다
                        rb.constraints = RigidbodyConstraints2D.None;
                }


                if (!isAttach) rb.constraints = RigidbodyConstraints2D.None;
            }
            else
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                if (caterpillarCtrl.onCtrl)
                {
                    rb.gravityScale = 0f;
                    rb.totalForce = Vector2.zero;
                }
                else rb.gravityScale = 1f;
            }
        }
    }
}
