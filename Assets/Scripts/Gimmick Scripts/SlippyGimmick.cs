using UnityEditor.SceneManagement;
using UnityEngine;

public class SlippyGimmick : MonoBehaviour
{
    [SerializeField] private float friction; // 마찰력 정도

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
        // 플래이어 접촉시 홀드 해제 코드 => 홀드를 마찰력으로 적용했을경우 필요 X
    }
}
