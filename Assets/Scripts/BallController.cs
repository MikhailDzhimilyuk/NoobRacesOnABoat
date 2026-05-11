using UnityEngine;

public class BallController : MonoCache
{
    public float speed = 0.1f;
    private float time = 0;
    private float posY;
    private bool canFall = true;
    public Vector3 targetPos = MyPlayerController.PlayerLocalPosition;

    private void Start()
    {
        if (transform.parent.parent.localEulerAngles.z > 29) 
        {
            canFall = false;
            targetPos.y -= 0.5f;
        } 
    }

    public override void OnFixedTick()
    {
        posY = transform.position.y;
        time += Time.fixedDeltaTime;
        float deltaX = transform.position.x - targetPos.x;
        float deltaY = transform.position.y - targetPos.y;

        if (deltaX < 0.1f && deltaX > -0.1f && deltaY < 0.1f && deltaY > -0.1f)
        {
            Destroy(gameObject, 0.1f);
        }

        else if (canFall)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - time * Time.fixedDeltaTime, transform.position.z);
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed);

        if (posY < transform.position.y && canFall)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + (posY - transform.position.y), transform.position.z);
        }

        Destroy(gameObject, 3f);
    }
}

