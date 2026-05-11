using System.Collections;
using UnityEngine;

public class HarpoonController : MonoCache
{
    [SerializeField] private GameObject Harpoon;
    [SerializeField] private GameObject Arrow;
    [SerializeField] private GameObject EtcSpawn;
    private AudioSource AudioArrowRelease;

    private static float maxDistanceShoot = 50f;
    private static float multiplierDelta = 0.5f;
    private float timeOutArrow = 0.3f;
    private bool canMove = false;
    private bool isLeftRow;
    private void Start()
    {
        if (transform.parent.parent.name.Contains("splitter")) { canMove = true; }

        AudioArrowRelease = GetComponent<AudioSource>();
        timeOutArrow = Random.Range(15, 31) / 24f;
        isLeftRow = (transform.parent.parent.localPosition.z > 10) ? false : true;

        if (!transform.parent.parent.name.Contains("splitter")) { transform.parent.localPosition = new Vector3(transform.parent.localPosition.x, 0, 0); }

       // StartCoroutine(CheckCanMove(1));
    }

    public override void OnFixedTick()
    {
        if ( (isLeftRow == MyPlayerController.isLeftRow || canMove) && !MyPlayerController.TakeDamage)
        {
            if (Mathf.Abs(MyPlayerController.PlayerLocalPosition.x - transform.position.x) < maxDistanceShoot)
            {
                float multiplierDistance = Mathf.Abs(Harpoon.transform.position.x - MyPlayerController.PlayerLocalPosition.x) / maxDistanceShoot;
                
                Vector3 targetPos = (MyPlayerController.PlayerVelocity.x > 0) ?
                MyPlayerController.PlayerLocalPosition - (MyPlayerController.PlayerLocalPosition - MyPlayerController.PlayerBackPos) * MyPlayerController.PlayerVelocity.x / MyPlayerController.maxVelocityBack * (multiplierDistance - (1 - multiplierDistance) / 20)
                : MyPlayerController.PlayerLocalPosition + (MyPlayerController.PlayerLocalPosition - MyPlayerController.PlayerForwardPos) * MyPlayerController.PlayerVelocity.x / MyPlayerController.maxVelocityForward * (multiplierDistance + (1 - multiplierDistance) / 10);
                targetPos += new Vector3(0, Mathf.Abs((MyPlayerController.PlayerLocalPosition.z - transform.parent.parent.localPosition.z) / 10f * MyPlayerController.PlayerVelocity.x / MyPlayerController.maxVelocityForward) , 0);
                // строчку выше пофиксить
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
                    GameObject arrow = Instantiate(Arrow, new Vector3(EtcSpawn.transform.position.x, EtcSpawn.transform.position.y, EtcSpawn.transform.position.z),
                        rotation * Quaternion.Euler(0, 0, 180), transform.parent.transform);

                    ArrowController arrowController = arrow.GetComponent<ArrowController>();
                    arrowController.targetPos = targetPos;
                    arrowController.speed = 0.08f;

                    float playerZ = MyPlayerController.PlayerLocalPosition.z - transform.parent.parent.localPosition.z;

                    if (transform.localPosition.y > 5.2 ) { if ( playerZ > 2.5f) arrowController.speed = 0.095f + playerZ / 225f; }

                    else { if (playerZ < -2.5f) arrowController.speed = 0.095f - playerZ / 225f; }

                    timeOutArrow = ( transform.parent.name.StartsWith("two_harpoons") ) ? Random.Range(15, 31) / 12f : Random.Range(15, 31) / 18f;

                    float DeltaX = Mathf.Abs(transform.position.x - MyPlayerController.PlayerLocalPosition.x);

                    if (DeltaX < 35)
                    {
                        AudioArrowRelease.volume = SettingsController.ResultVolumeSounds * ( 1f - DeltaX / 35f );
                        AudioArrowRelease.Play();
                    }
                }
            }
        }
        
    }
}