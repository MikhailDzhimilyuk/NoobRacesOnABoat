using UnityEngine;

public class SC_FPSCounter : MonoBehaviour
{
    public float updateInterval = 0.5f;

    float accum = 0.0f;
    int frames = 0;
    float timeleft;
    public static int fps;

    GUIStyle textStyle = new GUIStyle();

    void Start()
    {
        timeleft = updateInterval;
        textStyle.fontStyle = FontStyle.Bold;
        textStyle.normal.textColor = Color.white;
    }

    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        if (timeleft <= 0.0)
        {
            fps = (int)(accum / frames);
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }

    /*void OnGUI()
    {
        //Display the fps and round to 2 decimals
        GUI.Label(new Rect(5, 5, 100, 25), fps.ToString() + "FPS", textStyle);
    }*/
}