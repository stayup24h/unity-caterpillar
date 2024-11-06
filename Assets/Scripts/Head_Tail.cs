using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head_Tail : MonoBehaviour
{
    public bool isAttach = false;
    public GameObject attachedObject;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            isAttach = true;
            attachedObject = collision.gameObject;
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
