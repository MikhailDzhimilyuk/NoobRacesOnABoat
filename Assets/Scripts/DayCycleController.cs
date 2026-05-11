using UnityEngine;
using System.Collections;

public class DayCycleController : MonoBehaviour
{
    [ExecuteInEditMode]
    [SerializeField] private Light dirLight;
    [SerializeField] private AnimationCurve SkyBoxCurve;
    [SerializeField] private GameObject StarsGO;
    [SerializeField] private ParticleSystem Stars;

    [SerializeField] private Gradient dirLightGradient;
    [SerializeField] private Gradient ambientLightGradient;

    [SerializeField, Range(0, 1)] private float timeOfDay;
    [SerializeField, Range(1, 3600)] private float dayDuration;
    [SerializeField] private Material materialCoin;
    [SerializeField] private SettingsController settingsController;

    private float dayDurationHours;
    private float oneHour;
    private Vector3 defaultAngles;
    private Light MyLight;
    public float sunIntensity = 0.5f;
    public static float currentSunIntensity;
    private void Awake()
    {
        MyLight = GetComponent<Light>();
        oneHour = dayDuration / 24f;
        dayDurationHours = dayDuration / oneHour;
        defaultAngles = dirLight.transform.localEulerAngles;
        StartCoroutine(SetColorMaterialCoin(0));
    }


    private void FixedUpdate()
    {
        if (!PauseController.CheckIsPause() && SettingsController.canChangeTimeOfDay)
        {
            
            if (Application.isPlaying) { timeOfDay += Time.fixedDeltaTime / dayDuration; }

            if (timeOfDay >= 1f) { timeOfDay = 0f; }

            dirLight.transform.localEulerAngles = new Vector3(360f * timeOfDay - 90, defaultAngles.y, defaultAngles.z);
        }

        if (timeOfDay < 0.7 && timeOfDay > 0.3) { StarsGO.SetActive(false); } else if (!StarsGO.activeInHierarchy) { StarsGO.SetActive(true); }
    }

    private IEnumerator SetColorMaterialCoin(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        var mainModule = Stars.main;
        mainModule.startColor = new Color(1, 1, 1, 1 - SkyBoxCurve.Evaluate(timeOfDay));

        dirLight.color = dirLightGradient.Evaluate(timeOfDay);
        //float fogColor = 0.5f * timeOfDay + 0.2f;
        //RenderSettings.fogColor = new Color(fogColor, fogColor, fogColor);
        RenderSettings.ambientLight = ambientLightGradient.Evaluate(timeOfDay);
        MyLight.intensity = sunIntensity * SkyBoxCurve.Evaluate(timeOfDay);
        currentSunIntensity = MyLight.intensity;

        materialCoin.SetVector("_EmissionColor", new Vector4(0.501f, 0.281f, 0) * (MyLight.intensity * 0.7f + 0.3f));

        StartCoroutine(SetColorMaterialCoin(1));
    }

    public void SetTimeOfDay(float valueTime)
    {
        timeOfDay = valueTime;
        dirLight.transform.localEulerAngles = new Vector3(360f * timeOfDay - 90, defaultAngles.y, defaultAngles.z);

        var mainModule = Stars.main;
        mainModule.startColor = new Color(1, 1, 1, 1 - SkyBoxCurve.Evaluate(timeOfDay));

        dirLight.color = dirLightGradient.Evaluate(timeOfDay);

        RenderSettings.ambientLight = ambientLightGradient.Evaluate(timeOfDay);
        MyLight.intensity = sunIntensity * SkyBoxCurve.Evaluate(timeOfDay);
        currentSunIntensity = MyLight.intensity;
        settingsController.SetScrollbarTimeOfDay(timeOfDay);

        materialCoin.SetVector("_EmissionColor", new Vector4(0.501f, 0.281f, 0) * (MyLight.intensity * 0.7f + 0.3f));
    }
}
