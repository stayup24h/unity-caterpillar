using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head_Tail : MonoBehaviour
{
    public bool dead;
    public bool isAttach = false;
    public GameObject attachedObject;

    void Awake()
    {
        dead = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            isAttach = true;
            attachedObject = collision.gameObject;
        }
        if (collision.gameObject.CompareTag("Defeat"))
        {
            dead = true;
        }
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
}
