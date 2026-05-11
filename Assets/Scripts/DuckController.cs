using System.Collections;
using UnityEngine;

public class DuckController : MonoCache 
{
    private static float rightPos = 3.875f;
    private static float leftPos = -rightPos;
    private bool movingToLeft = true;
    private bool animRotateRight = true;
    private bool stopMoving = false;
    private float bias;
    private float rotZ;
    private static int multiplierRotZ = 70;
    private float multiplierBias = 100;
    private float speed = 2;
    private float localPosXParentPlatform;
    private bool canMove;

    private void Start()
    {
        float randomNum = Random.Range(1, 301) / 100f;
        localPosXParentPlatform = transform.parent.localPosition.x;
        speed = (speed + randomNum) * Time.fixedDeltaTime;
        multiplierBias = ( multiplierBias * (speed + randomNum) ) * Time.fixedDeltaTime;
       // Debug.Log(multiplierSpeed);
        //StartCoroutine(CheckCanMove(randomNum / 4f));
    }

    public override void OnFixedTick()
    {

        if (!stopMoving)
        {
            if (movingToLeft)
            {
                if (leftPos < transform.localPosition.z) { transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - speed); }

                else { movingToLeft = false; stopMoving = true; }
            }

            else
            {
                if (transform.localPosition.z < rightPos) { transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + speed); }

                else { movingToLeft = true; stopMoving = true; }
            }


            if (animRotateRight)
            {
                if (rotZ < 10)
                {
                    rotZ += Time.fixedDeltaTime * multiplierRotZ;
                }

                else { animRotateRight = false; }
            }

            else
            {
                if (rotZ > -10)
                {
                    rotZ -= Time.fixedDeltaTime * multiplierRotZ;
                }

                else { animRotateRight = true; }
            }
        }

        else
        {
            if (!movingToLeft)
            {
                if (bias <= 180) { bias += multiplierBias; }
                else { stopMoving = false; }
            }

            else
            {
                if (bias <= 360) { bias += multiplierBias; }
                else { stopMoving = false; bias = 0; }
            }
        }

        transform.localRotation = Quaternion.Euler(-90 + rotZ / 2, 0, rotZ + bias);
    }
}
