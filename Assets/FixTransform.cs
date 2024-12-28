using UnityEngine;

public class FixTransform : MonoBehaviour
{
    bool fix;
    Vector3 fixPosition;

    void Awake()
    {
        fixPosition = transform.position;
        fix = false;
    }

    [ContextMenu("FixPosition")]
    public void Fix() {
        fixPosition = transform.position;
        fix = !fix;
    }

    void Update()
    {
        if (fix) transform.position = fixPosition;
    }
}
