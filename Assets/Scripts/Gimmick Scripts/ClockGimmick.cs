using UnityEngine;

public class ClockGimmick : MonoBehaviour
{
    [SerializeField] private Transform minArrow;
    [SerializeField] private float speed;

    void Start()
    {
        if (minArrow == null) minArrow = transform.GetChild(0);
    }

    void FixedUpdate()
    {
        float delta = Time.time * speed;
        minArrow.transform.rotation = Quaternion.Euler(0, 0, - delta % 360);
    }
}
