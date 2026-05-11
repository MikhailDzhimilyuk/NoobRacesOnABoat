using UnityEngine;

public class EggController : MonoCache
{
    private float speed;
    private void Start()
    {
        speed = Random.Range(45, 65) / 24f * Time.fixedDeltaTime;
    }
    public override void OnFixedTick()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - speed, transform.localPosition.z);

        if (transform.localPosition.y < - 3) { Destroy(gameObject); }
    }
}
