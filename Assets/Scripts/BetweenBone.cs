using Unity.VisualScripting;
using UnityEngine;

public class BetweenBone : Bone
{
    public int upScale;
    public Transform backBone;
    Vector3 lastMove;

    public override void Initialize()
    {
        base.Initialize();
        lastMove = Vector2.zero;
    }

    protected override void Update()
    {
        /*
        base.Update();

        if (CaterpillarCtrl.turn == State.head || CaterpillarCtrl.turn == State.wait_tail)
        {
            Move_headTurn();
        }
        else if (CaterpillarCtrl.turn == State.tail || CaterpillarCtrl.turn == State.wait_head)
        {
            Move_tailTurn();
        }
        */
    }


    void Move_headTurn()
    {
        Vector3 movePosition = Vector3.zero;

        if (Vector3.Distance(transform.position, frontBone.position) > 1f)
            movePosition += (frontBone.position - transform.position) * Time.deltaTime * moveSpeed;
        if(Vector3.Distance(transform.position, backBone.position) > 1f)
            movePosition += (backBone.position - transform.position) * Time.deltaTime * moveSpeed;
        if (Vector3.Distance((lastMove * (-1)), movePosition) < 0.05f) return;

        if (movePosition.magnitude >= 0.02f) transform.position += movePosition;

        lastMove = movePosition;
    }

    void Move_tailTurn()
    {
        Vector3 normalizedDirection = direction.normalized;
        Vector3 normalDirection = new Vector3(-direction.y, direction.x, 0).normalized;

        Vector3 movePosition = Vector3.zero;
        float x = distance * (6 - position) / 6f;
        movePosition = tail.position + normalizedDirection * x;
        if (distance > 1.7f && distance < 5.8f)
        {
            switch (position)
            {
                case 1:
                    {
                        x += distance * 0.05f;
                        break;
                    }
                case 2:
                    {
                        x += distance * 0.04f;
                        break;
                    }
                case 3:
                    {
                        break;
                    }
                case 4:
                    {
                        x -= distance * 0.04f;
                        break;
                    }
                case 5:
                    {
                        x -= distance * 0.05f;
                        break;
                    }
            }
            float a = Mathf.Log((distance - 1.6f), Mathf.Pow(6.6f, 0.5f)) - 1.5f;
            movePosition = tail.position + normalizedDirection * x;
            movePosition += normalDirection * a * x * (x - distance);
            transform.position = movePosition;
        }
        else if (distance >= 5.8f)
        {
            transform.position = movePosition;
        }
        else if (distance > 1f)
        {
            switch (position)
            {
                case 1:
                    {
                        x += distance * 0.05f;
                        break;
                    }
                case 2:
                    {
                        x += distance * 0.04f;
                        break;
                    }
                case 3:
                    {
                        break;
                    }
                case 4:
                    {
                        x -= distance * 0.04f;
                        break;
                    }
                case 5:
                    {
                        x -= distance * 0.05f;
                        break;
                    }
            }
            float a = Mathf.Log((distance - 1f), Mathf.Pow(6.6f, 0.5f)) - 3.6f;
            movePosition = tail.position + normalizedDirection * x;
            movePosition += normalDirection * a * x * (x - distance);
            transform.position = movePosition;
        }
    }
}