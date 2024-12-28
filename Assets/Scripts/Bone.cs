using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bone : MonoBehaviour
{
    public static Vector2 input;
    public static Vector3 direction;
    public static float distance;
    public static Transform head, tail;

    public static float moveSpeed;

    public Transform frontBone;
    public int position;

    protected Rigidbody2D rb;

    void Awake()
    {
        Time.timeScale = 0f;    
    }
    void Start()
    {
        Initialize();
    }
    public virtual void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        moveSpeed = 5.0f;
        Time.timeScale = 1f;
    }


    protected virtual void Update()
    {
        if (input == Vector2.zero) rb.gravityScale = 0f;
        else rb.gravityScale = 0.01f;
    }

    public static void Calculate(Vector2 input)
    {
        Bone.input = input;
        direction = head.position - tail.position;
        distance = direction.magnitude;
    }
}
