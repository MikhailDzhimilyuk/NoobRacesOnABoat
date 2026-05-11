using System.Collections;
using UnityEngine;
using TMPro;
public class MyPlayerController : MonoCache
{
    [SerializeField] private AudioSource AudioTakeDamage;
    [SerializeField] private AudioSource AudioArrowStrike;
    [SerializeField] private AudioSource AudioBallHit;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private PhysicMaterial physicMaterial;
    [SerializeField] private GameObject playerWrap;
    [SerializeField] private GameObject EtcForward;
    [SerializeField] private GameObject EtcBack;

    [SerializeField] private GameObject EtcForwardUp;
    [SerializeField] private GameObject EtcBackUp;
    [SerializeField] private GameObject EtcUp;

    [SerializeField] private GameObject Head;
    [SerializeField] private GameObject StarsDamage;
    [SerializeField] private static AudioSource AudioTakingCoin;
    [SerializeField] private TextMeshProUGUI TextCoinsLevel;
    [SerializeField] private TextMeshProUGUI TextTimeLevel;
    [SerializeField] private GameObject starsDamageWrap;
    [SerializeField] private GameObject EtcStars;
    [SerializeField] private GameObject CanvasResult;
    [SerializeField] private TextMeshProUGUI TextHeartsLevel;
    [SerializeField] private GameObject JoystickGO;
    [SerializeField] private GameObject JoystickCamera;
    [SerializeField] private Joystick joystick;
    [SerializeField] private GameObject CanvasLevel;
    [SerializeField] private GameObject PanelMenuPC;
    [SerializeField] private GameObject SettingsMobile;
    [SerializeField] private GameObject BackToHome;
    [SerializeField] private PauseController pauseController;
    [SerializeField] private GameObject CanvasChooseLevel;
    [SerializeField] private GameObject CanvasShop;
    [SerializeField] private GameObject StarsGameWorld;
    public static int heartsCounter;
    public static int maxVelocityForward = 12;
    public static int maxVelocityBack = 5;
    public static bool TakeDamage = false;
    private int maxVelocityRight = 1;
    private float velocityRightMultiplier = 1;
    private float prevRotateState;
    public static Vector3 PlayerLocalPosition; // то же что и в мировых, поскольку игрок не вложен в какой-либо объект
    public static Vector3 PlayerForwardPos;
    public static Vector3 PlayerBackPos;
    public static Vector3 PlayerVelocity;
    public static bool isLeftRow = true;
    public static int coinsLevel = 0;
    public static int timeLevelCounter = 0;
    public static bool needUpdateCoinCounter = false;
    private bool isRotateDown = true;
    private float rotZ;

    public static Vector3 PlayerUpPos;
    public static Vector3 PlayerBackUpPos;
    public static Vector3 PlayerForwardUpPos;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Application.isMobilePlatform) 
        { 
            JoystickGO.SetActive(true);
            JoystickCamera.SetActive(true);
            SettingsMobile.SetActive(true);
            BackToHome.SetActive(true);
        }

        AudioTakingCoin = GameObject.FindGameObjectWithTag("AudioCoins1").GetComponent<AudioSource>();
        //rb.solverIterations = 9;
    }

    // Update is called once per frame
    public override void OnFixedTick()
    {
        if (needUpdateCoinCounter)
        {
            TextCoinsLevel.text = coinsLevel.ToString();
            needUpdateCoinCounter = false;
        }

        if (TakeDamage)
        {
            if (transform.eulerAngles.z < 32 && transform.eulerAngles.z > 28) { starsDamageWrap.transform.localEulerAngles = new Vector3(0, 0, 30); }
            else { starsDamageWrap.transform.localEulerAngles = new Vector3(0, 0, 0); }

            StarsDamage.transform.localEulerAngles = new Vector3(StarsDamage.transform.localEulerAngles.x, StarsDamage.transform.localEulerAngles.y, StarsDamage.transform.localEulerAngles.z + Time.fixedDeltaTime * 50);
        }

        isLeftRow = (transform.position.z > 7) ? false : true;

        PlayerForwardPos = EtcForward.transform.position;
        PlayerBackPos = EtcBack.transform.position;

        PlayerForwardUpPos = EtcForwardUp.transform.position;
        PlayerBackUpPos = EtcBackUp.transform.position;
        PlayerUpPos = EtcUp.transform.position;

        PlayerVelocity = rb.velocity;
        PlayerLocalPosition = transform.localPosition;
        // rotZ = (transform.localEulerAngles.z < 180) ? transform.localEulerAngles.z : -(transform.localEulerAngles.z - 180);
        // Получаем ввод от игрока

        /* rotZ = (transform.localEulerAngles.z < 180) ? transform.localEulerAngles.z : -(transform.localEulerAngles.z - 180);

         if (rotZ >= 46 && rotZ <= 180)
         {
             rb.MoveRotation(Quaternion.Euler(0, 0, 45));
         }*/

        float horizontalInput;
        float verticalInput;
        

        if (Application.isMobilePlatform)
        {
            horizontalInput = joystick.Horizontal * velocityRightMultiplier;
            verticalInput = -joystick.Vertical;
        }

        else
        {
            horizontalInput = Input.GetAxis("Horizontal") * velocityRightMultiplier;
            verticalInput = -Input.GetAxis("Vertical");
        }
        

        if (PauseController.CheckIsPause() == false && CanvasLevel.activeInHierarchy && !CanvasResult.activeInHierarchy)
        {
            Vector3 moveForward = new Vector3(1, 0, 0) * verticalInput;
            physicMaterial.dynamicFriction = 0.1f;
            physicMaterial.staticFriction = 0.1f;
            EtcForward.transform.localPosition = new Vector3(-0.25f, 0, 0);

            if (transform.localEulerAngles.z > 10 && transform.localEulerAngles.z < 80)
            {
                EtcForward.transform.localPosition = new Vector3(-0.28f, 0, 0);
                moveForward = new Vector3(-1, 0, 0);
                physicMaterial.dynamicFriction = 0;
                physicMaterial.staticFriction = 0;
            }

            Vector3 moveRight = new Vector3(0, 0, 1) * horizontalInput;

            int horizontalDir = (rb.velocity.x > 0.3 && verticalInput > 0) ? -1 : 1;
            float calcRotY = prevRotateState + horizontalInput * horizontalDir * 5 - (prevRotateState * (2 - Mathf.Abs(verticalInput)) / 6);
            float RotY = (calcRotY > 18) ? 18 : (calcRotY < -18) ? -18 : calcRotY;
            playerWrap.transform.localRotation = Quaternion.Euler(0, RotY, 0);
            prevRotateState = RotY;

            rb.AddForce(moveForward * 2700 * Time.fixedDeltaTime, ForceMode.Impulse);
            rb.AddForce(moveRight * 1000 * Time.fixedDeltaTime, ForceMode.VelocityChange);

        }

        if (rb.velocity.x >= maxVelocityBack)
        {
            rb.velocity = new Vector3(rb.velocity.normalized.x * maxVelocityBack, rb.velocity.y, rb.velocity.z);
        }

        if (rb.velocity.x <= -maxVelocityForward)
        {
            rb.velocity = new Vector3(rb.velocity.normalized.x * maxVelocityForward, rb.velocity.y, rb.velocity.z);
        }

        if (Mathf.Abs(rb.velocity.z) >= maxVelocityRight)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.normalized.z * maxVelocityRight);
        }

    }

    public override void OnTick()
    {
        StarsGameWorld.transform.rotation = Quaternion.Euler(0, 0, 0);
        StarsDamage.transform.position = EtcStars.transform.position;
        //StarsDamage.transform.position = new Vector3(Head.transform.position.x + 0.055f, Head.transform.position.y + 0.42f, Head.transform.position.z);
        // Debug.Log("OnTick");
        if (Input.GetKeyDown(KeyCode.Tab) && !CanvasChooseLevel.activeInHierarchy && !CanvasShop.activeInHierarchy)
        {
            PauseController.SetPause();
            PanelMenuPC.SetActive(true);
            pauseController.ShowCursor();
        }

        /* if (rotationObj.transform.localEulerAngles.z >= 0.3f && rotationObj.transform.localEulerAngles.z <= 29.7f && rotationObj.transform.localEulerAngles.z <= 350)
         {
             //  transform.localRotation = Quaternion.Euler(0, 0, rotationObj.transform.localRotation.z);
             rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
         }

         else
         {
             rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
         }*/

        //Debug.Log(transform.position.x);
    }

    public void ClearPlayer()
    {
        rb.velocity = new Vector3(0, 0, 0);
        rb.rotation = Quaternion.Euler(0, 0, 0);
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        TextCoinsLevel.text = "0";
        coinsLevel = 0;
        needUpdateCoinCounter = false;
        timeLevelCounter = Levels.timeOfLevels[GameWorld.currentLevel];
        heartsCounter = 20;
        TextTimeLevel.text = timeLevelCounter.ToString();
        TextHeartsLevel.text = heartsCounter.ToString();

        StopAllCoroutines();

        maxVelocityForward = 12;
        maxVelocityBack = 5;
        velocityRightMultiplier = 1;
        StarsDamage.SetActive(false);
        TakeDamage = false;
    }

    public void TickTack()
    {
        StartCoroutine(UpdateTimeLevel());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name.Contains("arrow"))
        {
            AudioArrowStrike.volume = SettingsController.ResultVolumeSounds * 0.35f;
            AudioArrowStrike.Play();

            StartCoroutine(PlayerTakeDamage());

            AudioTakeDamage.volume = SettingsController.ResultVolumeSounds;
            AudioTakeDamage.PlayDelayed(0.1f);
        }

        if (other.transform.name.Contains("ball"))
        {
            StartCoroutine(PlayerTakeDamage());

            AudioBallHit.volume = SettingsController.ResultVolumeSounds * 0.25f;
            AudioBallHit.Play();

            StartCoroutine(PlayerTakeDamage());

            AudioTakeDamage.volume = SettingsController.ResultVolumeSounds;
            AudioTakeDamage.PlayDelayed(0.1f);
        }

        if (other.transform.name.Contains("duck") || other.transform.name.Contains("egg"))
        {
            Destroy(other.gameObject);
            StartCoroutine(PlayerTakeDamage());
            AudioTakeDamage.volume = SettingsController.ResultVolumeSounds;
            AudioTakeDamage.PlayDelayed(0.1f);
        }

        if (other.transform.name.StartsWith("LockRotateModule") || other.transform.name.StartsWith("splitter_open"))
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        if (other.transform.name.StartsWith("UnLockRotateModule") || other.transform.name.StartsWith("splitter_close"))
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        }

        if (other.transform.name.Contains("coin"))
        {
            CoinController coinController = other.gameObject.GetComponentInChildren<CoinController>();
            coinController.isCollect = true;
            Destroy(other.gameObject, 0.02f);
        }

        if (other.transform.tag.Contains("Finish"))
        {
            CanvasResult.SetActive(true);
        }
    }

    public static void PlayAudioTakingCoin()
    {
        AudioTakingCoin.volume = SettingsController.ResultVolumeSounds * 0.1f;
        AudioTakingCoin.Play();
    }

    private IEnumerator PlayerTakeDamage()
    {
        if (!TakeDamage && heartsCounter > 0) { heartsCounter -= 1; TextHeartsLevel.text = heartsCounter.ToString(); }

        TakeDamage = true;
        StarsDamage.SetActive(true);
        maxVelocityForward = 4;
        maxVelocityBack = 2;
        velocityRightMultiplier = 0.5f;

        yield return new WaitForSeconds(2);

        maxVelocityForward = 12;
        maxVelocityBack = 5;
        velocityRightMultiplier = 1;
        StarsDamage.SetActive(false);
        TakeDamage = false;
    }

    private IEnumerator UpdateTimeLevel()
    {
        yield return new WaitForSeconds(1);

        if (timeLevelCounter > 0) { timeLevelCounter -= 1; TextTimeLevel.text = timeLevelCounter.ToString(); }

        StartCoroutine(UpdateTimeLevel());
    }
}
