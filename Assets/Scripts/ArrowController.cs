using UnityEngine;

public class ArrowController : MonoCache
{
    public float speed = 0.1f;
    public Vector3 targetPos = MyPlayerController.PlayerLocalPosition;

    public override void OnFixedTick()
    {
        float deltaX = transform.position.x - targetPos.x;
        float deltaY = transform.position.y - targetPos.y;

        if (deltaX < 0.1f && deltaX > -0.1f && deltaY < 0.1f && deltaY > -0.1f)
        {
            Destroy(gameObject, 0.1f);
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed);

        Destroy(gameObject, 3f);
    }
}
