using UnityEngine;

public class TailBone : Bone
{
    public override void Initialize()
    {
        base.Initialize();
    }

    protected override void Update()
    {
        
    }

    private void Move()
    {
        if (Vector2.Distance(transform.position, frontBone.position) > 1f)
            transform.position += (frontBone.position - transform.position) * Time.deltaTime * moveSpeed;
    }
}
