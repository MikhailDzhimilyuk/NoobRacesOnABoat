using UnityEngine;
using YG;

public class MyShortcut : MonoBehaviour
{
    [SerializeField] private GameObject CanvasChooseLevel;


    private void Start()
    {
        Invoke("ShowShortcut", 300);
    }
    private void ShowShortcut()
    {
        if (CanvasChooseLevel.activeInHierarchy)
        {

            if (YandexGame.EnvironmentData.promptCanShow == true && YandexGame.savesData.promptDone == false) { YandexGame.PromptShow(); }

        }
        else Invoke("ShowShortcut", 10);
    }
}