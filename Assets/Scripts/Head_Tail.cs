using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head_Tail : MonoBehaviour
{
    public bool isAttach = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            isAttach = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            isAttach = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            isAttach = true;
        }
    }
}
