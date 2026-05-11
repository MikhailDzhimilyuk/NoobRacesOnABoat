using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class SettingsController : MonoBehaviour
{
    [SerializeField] private Scrollbar ScrollbarSounds;
    [SerializeField] private Scrollbar ScrollbarMusic;
    [SerializeField] private Image ImageButtonSounds;
    [SerializeField] private Image ImageButtonMusic;
    [SerializeField] private Image ImageScrollbarSounds;
    [SerializeField] private Image ImageScrollbarSoundsHandle;
    [SerializeField] private Image ImageScrollbarMusic;
    [SerializeField] private Image ImageScrollbarMusicHandle;
    [SerializeField] private AudioClip[] BackgroundMusics;
    [SerializeField] private AudioSource AudioBackground;
    [SerializeField] private GameObject CanvasSettings;

    [SerializeField] private GameObject ScrollbarTimeOfDayGO;
    [SerializeField] private Scrollbar ScrollbarTimeOfDay;
    [SerializeField] private Image ImageButtonTimeOfDay;
    [SerializeField] private Image ImageScrollbarTimeOfDay;
    [SerializeField] private Image ImageScrollbarTimeOfDayHandle;
    [SerializeField] private DayCycleController dayCycleController;
    [SerializeField] private Image ImageCameraRenderer;
    [SerializeField] private Image ImageScrollbarCameraRenderer;
    [SerializeField] private Image ImageScrollbarHandleCameraRenderer;
    [SerializeField] private Scrollbar ScrollbarCameraRenderer;
    [SerializeField] private Camera MainCamera;
    [SerializeField] private GameObject FPS_Checker;
    private int currentClip = 0;
    private static int toggleSounds = 1;
    private static int toggleMusic = 1;
    private float prevVolumeSounds = 0.5f;
    private float prevVolumeMusic = 0.5f;
    private static float volumeSounds = 0.55f;
    private static float volumeMusic = 0.55f;
    public static bool canChangeTimeOfDay = true;
    public static float ResultVolumeSounds;
    public static float ResultVolumeMusic;
    private float PrevResultVolumeSounds;
    int countPosValuesFPS = 0;
    int accumFPS = 0;
    private void Start()
    {
        currentClip = Random.Range(0, 2);

        StartCoroutine(PlayBackgroundMusic(1));

        if (PlayerPrefs.HasKey("volumeSounds"))
        {
            volumeSounds = PlayerPrefs.GetFloat("volumeSounds");
        }

        if (PlayerPrefs.HasKey("volumeMusic"))
        {
            volumeMusic = PlayerPrefs.GetFloat("volumeMusic");
        }

        if (PlayerPrefs.HasKey("toggleSounds"))
        {
            toggleSounds = PlayerPrefs.GetInt("toggleSounds");
        }

        if (PlayerPrefs.HasKey("toggleMusic"))
        {
            toggleMusic = PlayerPrefs.GetInt("toggleMusic");
        }

        toggleSounds = (volumeSounds > 0) ? 1 : 0;
        toggleMusic = (volumeMusic > 0) ? 1 : 0;

        UpdateDataAndImages();

        if (PlayerPrefs.HasKey("cameraRendererVolume"))
        {
            FPS_Checker.SetActive(false);
            //StartCoroutine(CheckFpsAndSetQuality(1));
            ChangeCameraRenderer(PlayerPrefs.GetFloat("cameraRendererVolume"));
        }

        else
        {
            StartCoroutine(CheckFpsAndSetQuality(1));
        }
    }

    public void ChangeSounds()
    {
        prevVolumeSounds = volumeSounds;
        volumeSounds = ScrollbarSounds.value;
        toggleSounds = (volumeSounds > 0) ? 1 : 0;

        UpdateDataAndImages();
    }

    public void MuteSounds() { prevVolumeSounds = ResultVolumeSounds; ResultVolumeSounds = 0; }
    public void UnmuteSounds() => ResultVolumeSounds = prevVolumeSounds;

    public void ChangeMusic()
    {
        prevVolumeMusic = volumeMusic;
        volumeMusic = ScrollbarMusic.value;
        toggleMusic = (volumeMusic > 0) ? 1 : 0;
        UpdateDataAndImages();
    }

    public void ToggleSounds()
    {
        if (toggleSounds == 1)
        {
            toggleSounds = 0;
        }

        else
        {
            if (prevVolumeSounds < 0.01f) prevVolumeSounds = 0.1f;

            toggleSounds = 1;
            ScrollbarSounds.value = prevVolumeSounds;
            volumeSounds = ScrollbarSounds.value;
        }

        UpdateDataAndImages();
    }

    public void ToggleMusic()
    {
        if (toggleMusic == 1)
        {
            toggleMusic = 0;
        }

        else
        {
            if (prevVolumeMusic < 0.01f) prevVolumeMusic = 0.1f;

            toggleMusic = 1;
            ScrollbarMusic.value = prevVolumeMusic;
            volumeMusic = ScrollbarMusic.value;
        }

        UpdateDataAndImages();
    }

    public void ToggleTimeOfDay()
    {
        canChangeTimeOfDay = !canChangeTimeOfDay;
        ImageButtonTimeOfDay.sprite = (canChangeTimeOfDay) ? Resources.Load<Sprite>("Images/empty_button") : Resources.Load<Sprite>("Images/empty_button_off");
        ScrollbarTimeOfDayGO.SetActive(canChangeTimeOfDay);
    }

    public float GetAudioBackgroundVolume()
    {
        return AudioBackground.volume;
    }

    public void SetAudioBackgroundVolume(float value)
    {
        AudioBackground.volume = value;
    }

    public void ChangeTimeOfDay()
    {
       dayCycleController.SetTimeOfDay(ScrollbarTimeOfDay.value);
       ImageButtonTimeOfDay.color = new Color(0.9f - ScrollbarTimeOfDay.value, 0.9f * ScrollbarTimeOfDay.value, 0, 1);
       ImageScrollbarTimeOfDay.color = new Color(0.9f - ScrollbarTimeOfDay.value, 0.9f * ScrollbarTimeOfDay.value, 0, 1);
       ImageScrollbarTimeOfDayHandle.color = new Color(0.8f - ScrollbarTimeOfDay.value, 0.8f * ScrollbarTimeOfDay.value, 0, 1);
    }

    public void SetScrollbarTimeOfDay(float value)
    {
        ScrollbarTimeOfDay.value = value;
    }

    public void ChangeCameraRenderer(float value = -1)
    {
        if (value >= 0) { ScrollbarCameraRenderer.value = value; }

        PlayerPrefs.SetFloat("cameraRendererVolume", ScrollbarCameraRenderer.value);

        MainCamera.farClipPlane = 50 + ScrollbarCameraRenderer.value * 80;
        int currentQualityLevel = QualitySettings.GetQualityLevel();

        if (ScrollbarCameraRenderer.value <= 0.167f) { if (currentQualityLevel != 5) { QualitySettings.SetQualityLevel(5, true); } }
        else if (ScrollbarCameraRenderer.value <= 0.333f) { if (currentQualityLevel != 4) { QualitySettings.SetQualityLevel(4, true); } }
        else if (ScrollbarCameraRenderer.value <= 0.499f) { if (currentQualityLevel != 3) { QualitySettings.SetQualityLevel(3, true); } }
        else if (ScrollbarCameraRenderer.value <= 0.667f) { if (currentQualityLevel != 2) { QualitySettings.SetQualityLevel(2, true); } }
        else if (ScrollbarCameraRenderer.value <= 0.833f) { if (currentQualityLevel != 1) { QualitySettings.SetQualityLevel(1, true); } }
        else { if (currentQualityLevel != 0) { QualitySettings.SetQualityLevel(0, true); } }

        ImageCameraRenderer.color = new Color(0.9f - ScrollbarCameraRenderer.value, 0.9f * ScrollbarCameraRenderer.value, 0, 1);
        ImageScrollbarCameraRenderer.color = new Color(0.9f - ScrollbarCameraRenderer.value, 0.9f * ScrollbarCameraRenderer.value, 0, 1);
        ImageScrollbarHandleCameraRenderer.color = new Color(0.8f - ScrollbarCameraRenderer.value, 0.8f * ScrollbarCameraRenderer.value, 0, 1);
    }

    private void UpdateDataAndImages()
    {
        ResultVolumeSounds = volumeSounds * toggleSounds;
        ResultVolumeMusic = volumeMusic * toggleMusic;

        ImageButtonSounds.color = new Color(0.9f - ResultVolumeSounds, 0.9f * ResultVolumeSounds, 0, 1);
        ImageScrollbarSounds.color = new Color(0.9f - ResultVolumeSounds, 0.9f * ResultVolumeSounds, 0, 1);
        ImageScrollbarSoundsHandle.color = new Color(0.8f - ResultVolumeSounds, 0.8f * ResultVolumeSounds, 0, 1);

        ImageButtonMusic.color = new Color(0.9f - ResultVolumeMusic, 0.9f * ResultVolumeMusic, 0, 1);
        ImageScrollbarMusic.color = new Color(0.9f - ResultVolumeMusic, 0.9f * ResultVolumeMusic, 0, 1);
        ImageScrollbarMusicHandle.color = new Color(0.8f - ResultVolumeMusic, 0.8f * ResultVolumeMusic, 0, 1);

        ImageButtonSounds.sprite = (ResultVolumeSounds > 0) ? Resources.Load<Sprite>("Images/sounds") : Resources.Load<Sprite>("Images/soundsOff");
        ImageButtonMusic.sprite = (ResultVolumeMusic > 0) ? Resources.Load<Sprite>("Images/music") : Resources.Load<Sprite>("Images/musicOff");

        ScrollbarSounds.value = ResultVolumeSounds;
        ScrollbarMusic.value = ResultVolumeMusic;

        PlayerPrefs.SetFloat("volumeSounds", volumeSounds);
        PlayerPrefs.SetInt("toggleSounds", toggleSounds);

        PlayerPrefs.SetFloat("volumeMusic", volumeMusic);
        PlayerPrefs.SetInt("toggleMusic", toggleMusic);

        
        AudioBackground.volume = ResultVolumeMusic;

        // if (ResultVolumeMusic == 0) { AudioBackground.Pause(); } else { AudioBackground.UnPause(); }
    }

    private IEnumerator PlayBackgroundMusic(int time)
    {
        yield return new WaitForSecondsRealtime(time);

        currentClip = (currentClip == 0) ? 1 : 0;
        AudioBackground.clip = BackgroundMusics[currentClip];
        AudioBackground.Play();
        //Debug.Log(AudioBackground.clip.length);

        yield return new WaitForSecondsRealtime(AudioBackground.clip.length );

        StartCoroutine(PlayBackgroundMusic(3));
    }

    private IEnumerator CheckFpsAndSetQuality(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        float result;

        if (SC_FPSCounter.fps > 0 && SC_FPSCounter.fps < 10000)
        {
            accumFPS += SC_FPSCounter.fps;
            countPosValuesFPS += 1;
           // Debug.Log("fps[" + countPosValuesFPS + "]: " + SC_FPSCounter.fps);
        }

        if (countPosValuesFPS == 5)
        {
            result = accumFPS / countPosValuesFPS;
            
            if (result >= 100) { ChangeCameraRenderer(1); }
            else if (result >= 75) { ChangeCameraRenderer(0.83f); }
            else if (result >= 60) { ChangeCameraRenderer(0.65f); }
            else if (result >= 47) { ChangeCameraRenderer(0.49f); }
            else if (result >= 37) { ChangeCameraRenderer(0.33f); }
            else { ChangeCameraRenderer(0); }

            FPS_Checker.SetActive(false);
        }

        else StartCoroutine(CheckFpsAndSetQuality(0.55f));
    }
}
