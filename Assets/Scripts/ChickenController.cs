using UnityEngine;
using System.Collections;

public class ChickenController : MonoCache
{
    [SerializeField] private GameObject Chicken;
    [SerializeField] private GameObject RightWing;
    [SerializeField] private GameObject LeftWing;
    [SerializeField] private GameObject RightFoot;
    [SerializeField] private GameObject LeftFoot;
    [SerializeField] private GameObject Egg;
    private static float rightPos = 6f;
    private static float leftPos = -rightPos;
    private float LeftZPos;
    private float RightZPos;
    private float localPosXParentPlatform;
    private float RotZ;
    private float FootRotX;
    private float ChickenRotX = -90;
    private float multiplierBias = 100;
    private bool upWing = true;
    private bool isWalking = true;
    private bool stopMoving = false;
    private bool canMove = false;
    private bool movingToLeft = true;
    private float bias;
    private float timeOutEggs = 0.3f;

    private float speedWingRotate = 4;
    private float speedWalking = 4;
    private float speedFlying = 1;
    private void Start()
    {
        float randomNum = Random.Range(1, 301) / 100f;
        localPosXParentPlatform = transform.parent.localPosition.x;
        speedWingRotate += randomNum;
        multiplierBias *= (speedWingRotate * Time.fixedDeltaTime);
        speedWalking = Time.fixedDeltaTime * 16 * (speedWingRotate / 4);
        speedFlying = Time.fixedDeltaTime * speedWingRotate;
        speedWingRotate = Time.fixedDeltaTime * 312 * (speedWingRotate / 4);
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        LeftZPos = -4.7f + transform.parent.position.z;
        RightZPos = 4.7f + transform.parent.position.z;
    }

    public override void OnFixedTick()
    {
        if (upWing)
        {
            if (RotZ < 110) { RotZ += speedWingRotate; }
            else { upWing = false; }
        }

        else
        {
            if (RotZ >= 0) { RotZ -= speedWingRotate; }
            else { upWing = true; }
        }

        if (isWalking)
        {
            if (ChickenRotX < -87.5) { ChickenRotX += speedWalking; }
            else { isWalking = false; }
        }

        else
        {
            if (ChickenRotX > -92.5) { ChickenRotX -= speedWalking; }
            else { isWalking = true; }
        }

        if (!stopMoving)
        {
            if (movingToLeft)
            {
                if (leftPos < transform.localPosition.z) { transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - speedFlying); }

                else { movingToLeft = false; stopMoving = true; }

            }

            else
            {
                if (transform.localPosition.z < rightPos) { transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + speedFlying); }

                else { movingToLeft = true; stopMoving = true; }
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

        FootRotX = (ChickenRotX + 90) * 5;
        Chicken.transform.localRotation = Quaternion.Euler(ChickenRotX, 0, 0);
        transform.localRotation = Quaternion.Euler(0, bias, 0);
        RightWing.transform.localRotation = Quaternion.Euler(90, 0, -RotZ);
        LeftWing.transform.localRotation = Quaternion.Euler(90, 0, RotZ);

        RightFoot.transform.localRotation = Quaternion.Euler(FootRotX, 0, 0);
        LeftFoot.transform.localRotation = Quaternion.Euler(-FootRotX, 0, 0);

        if (transform.position.z < RightZPos && transform.position.z > LeftZPos)
        {
            if (timeOutEggs > 0)
            {
                timeOutEggs -= Time.fixedDeltaTime;
            }

            else
            {
                Instantiate(Egg, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(-90, 0, 0), transform.parent.transform);
                timeOutEggs = Random.Range(15, 45) / 100f;
            }
        }
    }
}
