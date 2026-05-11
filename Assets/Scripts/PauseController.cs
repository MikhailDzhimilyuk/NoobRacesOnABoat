using UnityEngine;

public class PauseController : MonoBehaviour
{
    private static bool isPause = false;
    private static AudioSource AudioBackGround;
    [SerializeField] private GameObject CanvasChooseLevel;
    [SerializeField] private GameObject CanvasShop;

    private void Awake()
    {
      //  AudioBackGround = GameObject.FindGameObjectWithTag("SettingsController").GetComponent<AudioSource>();
    }

    public static bool CheckIsPause()
    {
        return isPause;
    }

    public static void SetPause()
    {
        //AudioBackGround.Pause();
        isPause = true;
        Time.timeScale = 0;
    }

    public static void SetUnpause()
    {
       // AudioBackGround.UnPause();
        isPause = false;
        Time.timeScale = 1;
    }

    public void HideCursor()
    {
        if (!CanvasChooseLevel.activeInHierarchy && !CanvasShop.activeInHierarchy && !Application.isMobilePlatform) 
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowCursor()
    {
        if (!Application.isMobilePlatform)
        Cursor.lockState = CursorLockMode.Confined;
    }
}
