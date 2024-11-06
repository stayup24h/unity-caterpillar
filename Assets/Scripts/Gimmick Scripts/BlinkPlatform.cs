using UnityEngine;

public class BlinkPlatform : MonoBehaviour
{
    private CaterpillarCtrl caterpillarCtrl;
    private Rigidbody2D head_rb;
    private Rigidbody2D tail_rb;
    private bool isAttach;

    void Start()
    {
        caterpillarCtrl = FindObjectsByType<CaterpillarCtrl>(FindObjectsSortMode.None)[0];
        head_rb = caterpillarCtrl.transform.GetChild(0).GetComponent<Rigidbody2D>();
        tail_rb = caterpillarCtrl.transform.GetChild(6).GetComponent<Rigidbody2D>();
    }

    public void PlatformDisappear()
    {
        if (isAttach)
        {
            head_rb.constraints = RigidbodyConstraints2D.None;
            tail_rb.constraints = RigidbodyConstraints2D.None;
            isAttach = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            isAttach = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            isAttach = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            isAttach = false;
    }
}
