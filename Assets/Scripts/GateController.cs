using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoCache
{
    [SerializeField] private GameObject RightWall;
    [SerializeField] private GameObject LeftWall;
    private float StartPosZ;
    private float StartHandScaleY;
    private float maxScaleZ;
    private static float minScaleZ = 15f;
    private float newScaleZ;
    private float speed;
    private bool isExpand = true;
    private bool canMove;

    private void Start()
    {
        transform.localEulerAngles = new Vector3(0, 0, 0);
        speed = Random.Range(64, 128) * Time.fixedDeltaTime;
        maxScaleZ = RightWall.transform.parent.localScale.z;
        StartPosZ = RightWall.transform.parent.localPosition.z;
        StartHandScaleY = RightWall.transform.localScale.y;
       // StartCoroutine(CheckCanMove((speed - 62) / 48f));
    }

    public override void OnFixedTick()
    {
        if (true || canMove)
        {
            if (!isExpand)
            {
                if (newScaleZ >= minScaleZ) {
                    newScaleZ = RightWall.transform.parent.localScale.z - speed; 
                }

                else { isExpand = true; }
            }

            else
            {
                if (newScaleZ <= maxScaleZ) { newScaleZ = RightWall.transform.parent.localScale.z + speed; }

                else { isExpand = false; }
            }

            float newPosZ = StartPosZ + (maxScaleZ - newScaleZ) / 100f;
            RightWall.transform.parent.localScale = LeftWall.transform.parent.localScale = new Vector3(RightWall.transform.parent.localScale.x, RightWall.transform.parent.localScale.y, newScaleZ);
            RightWall.transform.parent.localPosition = new Vector3(RightWall.transform.parent.localPosition.x, RightWall.transform.parent.localPosition.y, newPosZ);
            LeftWall.transform.parent.localPosition = new Vector3(LeftWall.transform.parent.localPosition.x, LeftWall.transform.parent.localPosition.y, -newPosZ);
            RightWall.transform.localScale = LeftWall.transform.localScale = new Vector3(RightWall.transform.localScale.x, StartHandScaleY * maxScaleZ / RightWall.transform.parent.localScale.z, RightWall.transform.localScale.z);
        }
    }

    /*private IEnumerator CheckCanMove(float time)
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
