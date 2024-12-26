using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ClockGimmick : MonoBehaviour
{
    [SerializeField] private Transform minArrow;
    [SerializeField] private Transform hourArrow;
    [SerializeField] private float speed;

    void Start()
    {
        if (minArrow == null) minArrow = transform.GetChild(0);
        if (hourArrow == null) hourArrow = transform.GetChild(1);
    }

    void FixedUpdate()
    {
        float delta = Time.time * speed % (360 * 12);
        minArrow.transform.rotation = Quaternion.Euler(0, 0, - delta % 360);
        hourArrow.transform.rotation = Quaternion.Euler(0, 0, - delta / 12 % 360);
    }
}
