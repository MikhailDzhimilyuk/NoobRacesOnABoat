using UnityEngine;
using System.Collections;

public class HandController : MonoCache
{
    private float StartPosZ;
    private float StartHandScaleX;
    private float maxScaleZ;
    private static float minScaleZ = 35f;
    private float newScaleZ;
    private float speed;
    private bool isExpand = true;
    private bool canMove;

    private void Start()
    {
        transform.parent.parent.localEulerAngles = (transform.parent.parent.name.Contains("left")) ? new Vector3(0, 180, 0) : new Vector3(0, 0, 0);

        if (!transform.parent.parent.parent.name.Contains("splitter")) { transform.parent.parent.localPosition = new Vector3(transform.parent.parent.localPosition.x, 0, 0); }
        
        speed = Random.Range(64, 128) * Time.fixedDeltaTime;
        maxScaleZ = transform.parent.localScale.z;
        StartPosZ = transform.parent.localPosition.z;
        StartHandScaleX = transform.localScale.x;
        //StartCoroutine(CheckCanMove((speed - 62) / 48f));
    }

    public override void OnFixedTick()
    {
        if (true || canMove) 
        {
            if (!isExpand)
            {
                if (newScaleZ >= minScaleZ) { newScaleZ = transform.parent.localScale.z - speed; }

                else { isExpand = true; }
            }

            else
            {
                if (newScaleZ <= maxScaleZ) { newScaleZ = transform.parent.localScale.z + speed; }

                else { isExpand = false; }
            }

            transform.parent.localScale = new Vector3(transform.parent.localScale.x, transform.parent.localScale.y, newScaleZ);
            transform.parent.localPosition = new Vector3(transform.parent.localPosition.x, transform.parent.localPosition.y, StartPosZ + (maxScaleZ - newScaleZ) / 100f);
            transform.localScale = new Vector3(StartHandScaleX * maxScaleZ / transform.parent.localScale.z, transform.localScale.y, transform.localScale.z);
        }
    }

   /* private IEnumerator CheckCanMove(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        if (Mathf.Abs(MyPlayerController.PlayerLocalPosition.x - transform.position.x) < 100)
        {
            canMove = true;
        }

        else
        {
            canMove = false;
        }

        StartCoroutine(CheckCanMove(4));
    }*/
}
