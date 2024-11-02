using UnityEditor.SceneManagement;
using UnityEngine;

public class SlippyGimmick : MonoBehaviour
{
    [SerializeField] private float friction; // ������ ����

    private BoxCollider2D box;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        PhysicsMaterial2D physics = new PhysicsMaterial2D();
        physics.friction = friction;
        physics.bounciness = 0;
        box.sharedMaterial = physics;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �÷��̾� ���˽� Ȧ�� ���� �ڵ� => Ȧ�带 ���������� ����������� �ʿ� X
    }
}
