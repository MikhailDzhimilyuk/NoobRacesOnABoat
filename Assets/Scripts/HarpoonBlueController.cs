using System.Collections;
using UnityEngine;

public class HarpoonBlueController : MonoCache
{
    [SerializeField] private GameObject Harpoon;
    [SerializeField] private GameObject[] Balls;
    [SerializeField] private GameObject EtcSpawn;
    private AudioSource AudioBallRelease;

    private static float maxDistanceShoot = 50f;
    private static float multiplierDelta = 0.5f;
    private float timeOutArrow = 0.3f;
    private bool canMove = false;
    private bool isLeftRow;
    private void Start()
    {
        if (transform.parent.parent.name.Contains("splitter")) { canMove = true; }

        AudioBallRelease = GetComponent<AudioSource>();
        timeOutArrow = Random.Range(15, 31) / 24f;
        isLeftRow = (transform.parent.parent.localPosition.z > 10) ? false : true;

        if (!transform.parent.parent.name.Contains("splitter")) { transform.parent.localPosition = new Vector3(transform.parent.localPosition.x, 0, 0); }

        //StartCoroutine(CheckCanMove(1));
    }

    public override void OnFixedTick()
    {
        if ( (isLeftRow == MyPlayerController.isLeftRow || canMove) && !MyPlayerController.TakeDamage)
        {
            if (Mathf.Abs(MyPlayerController.PlayerUpPos.x - transform.position.x) < maxDistanceShoot)
            {
                float multiplierDistance = Mathf.Abs(Harpoon.transform.position.x - MyPlayerController.PlayerUpPos.x) / maxDistanceShoot;

                Vector3 targetPos = (MyPlayerController.PlayerVelocity.x > 0) ?
                MyPlayerController.PlayerUpPos - (MyPlayerController.PlayerUpPos - MyPlayerController.PlayerBackUpPos) * MyPlayerController.PlayerVelocity.x / MyPlayerController.maxVelocityBack * (multiplierDistance - (1 - multiplierDistance) / 20)
                : MyPlayerController.PlayerUpPos + (MyPlayerController.PlayerUpPos - MyPlayerController.PlayerForwardUpPos) * MyPlayerController.PlayerVelocity.x / MyPlayerController.maxVelocityForward * (multiplierDistance + (1 - multiplierDistance) / 10);
                targetPos += new Vector3(0, Mathf.Abs((MyPlayerController.PlayerUpPos.z - transform.parent.parent.localPosition.z) / 10f * MyPlayerController.PlayerVelocity.x / MyPlayerController.maxVelocityForward), 0);

                var rotation = Quaternion.LookRotation(targetPos - Harpoon.transform.position, Vector3.up);
                rotation *= Quaternion.Euler(0, 90, 0);
                Harpoon.transform.rotation = rotation;

                if (timeOutArrow > 0)
                {
                    timeOutArrow -= Time.fixedDeltaTime;
                }

                else
                {
                    // Debug.Log(multiplierDistanse);
                    GameObject ball = Instantiate(Balls[Random.Range(0, Balls.Length)], new Vector3(EtcSpawn.transform.position.x, EtcSpawn.transform.position.y, EtcSpawn.transform.position.z),
                        rotation * Quaternion.Euler(0, 0, 180), transform.parent.transform);

                    BallController ballController = ball.GetComponent<BallController>();
                    ballController.targetPos = targetPos;

                    ballController.speed = 0.06f;

                    float playerZ = MyPlayerController.PlayerUpPos.z - transform.parent.parent.localPosition.z;

                    if (transform.localPosition.y > 5.2) { if (playerZ > 2.5f) ballController.speed = 0.04f + playerZ / 225f; }

                    else { if (playerZ < -2.5f) ballController.speed = 0.055f - playerZ / 225f; }

                    timeOutArrow = (transform.parent.name.StartsWith("two_harpoons")) ? Random.Range(15, 54) / 48f : Random.Range(15, 54) / 72f;

                    float DeltaX = Mathf.Abs(transform.position.x - MyPlayerController.PlayerLocalPosition.x);

                    if (DeltaX < 35)
                    {
                        AudioBallRelease.volume = SettingsController.ResultVolumeSounds * (1f - DeltaX / 35f);
                        AudioBallRelease.Play();
                    }

                    /*ballController.speed = 0.08f;

                    if (transform.parent.parent.localEulerAngles.z > 29) { ballController.speed = 0.09f; }

                    float playerZ = MyPlayerController.PlayerUpPos.z - transform.parent.parent.localPosition.z;

                    if (transform.localPosition.y > 5.2) { if (playerZ > 2.5f) ballController.speed = 0.06f + playerZ / 225f; }

                    else { if (playerZ < -2.5f) ballController.speed = 0.09f - playerZ / 225f; }

                    timeOutArrow = (transform.parent.name.StartsWith("two_harpoons")) ? Random.Range(15, 42) / 18f : Random.Range(15, 42) / 24f;*/
                }
            }
        }

    }

   /* private IEnumerator CheckCanMove(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        if (Mathf.Abs(MyPlayerController.PlayerUpPos.x - localPosXParentPlatform) < 200 && (isLeftRow == MyPlayerController.isLeftRow || transform.parent.parent.name.Contains("splitter")))
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