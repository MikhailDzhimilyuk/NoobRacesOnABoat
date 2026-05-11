using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChooseLevelController : MonoBehaviour
{
    [SerializeField] private GameObject[] ButtonsGameObject = new GameObject[24];
    [SerializeField] private GameObject CanvasLevel;
    [SerializeField] private GameObject CanvasChooseLevel;
    [SerializeField] private GameWorld gameWorld;
    [SerializeField] private TextMeshProUGUI TextCounterAllStars;
    [SerializeField] private AdsManager adsManager;
    private Button[] Buttons = new Button[24];
    private Image[] ButtonsImages = new Image[24];
    private Image[] ButtonsImagesStars = new Image[24];
    private int[] StarsCounters = new int[24];

    public void StartGame()
    {
        for (int i = 0; i < ButtonsGameObject.Length; i++)
        {
            Buttons[i] = ButtonsGameObject[i].GetComponent<Button>();
            ButtonsImages[i] = Buttons[i].image;
            ButtonsImagesStars[i] = ButtonsGameObject[i].transform.GetChild(1).GetComponent<Image>();
        }

        UpdateInfoLevels();
    }

    public void GetLoad(int[] StarsCountersYG)
    {
        for(int i = 0; i < StarsCountersYG.Length; i++) { StarsCounters[i] = StarsCountersYG[i]; }
    }

    public void SetStarCounters(int index, int value)
    {
        StarsCounters[index] = value;
    }

    public int GetStarsCounter(int index)
    {
        return StarsCounters[index];
    }

    public int GetSummaryStarsCounter()
    {
        int counter = 0;

        for (int i = 0; i < StarsCounters.Length; i++) { counter += StarsCounters[i]; }

        return counter;
    }

    public void UpdateInfoLevels()
    {
        ButtonsImages[0].color = (StarsCounters[0] == 0) ? new Color32(168, 168, 168, 235) : new Color32(0, 198, 21, 221);
        ButtonsImagesStars[0].sprite = (StarsCounters[0] == 0) ? Resources.Load<Sprite>("Images/0_stars") : 
            (StarsCounters[0] == 1) ? Resources.Load<Sprite>("Images/1_stars") :
            (StarsCounters[0] == 2) ? Resources.Load<Sprite>("Images/2_stars") : 
            Resources.Load<Sprite>("Images/3_stars");

        for (int i = 1; i < ButtonsGameObject.Length; i++)
        {
            if (StarsCounters[i] == 0 && StarsCounters[i - 1] == 0)
            { 
                ButtonsImages[i].color = new Color32(197, 0, 0, 235); 
                ButtonsImagesStars[i].sprite = Resources.Load<Sprite>("Images/0_stars");
                Buttons[i].enabled = /*true;*/ false; 
            }

            if (StarsCounters[i] == 0 && StarsCounters[i - 1] > 0)
            {
                ButtonsImages[i].color = new Color32(168, 168, 168, 235);
                ButtonsImagesStars[i].sprite = Resources.Load<Sprite>("Images/0_stars");
                Buttons[i].enabled = true;
            }

            if (StarsCounters[i] >= 1)
            {
                ButtonsImages[i].color = new Color32(0, 198, 21, 221);
                ButtonsImagesStars[i].sprite = (StarsCounters[i] == 1) ? Resources.Load<Sprite>("Images/1_stars") :
                    (StarsCounters[i] == 2) ? Resources.Load<Sprite>("Images/2_stars") : Resources.Load<Sprite>("Images/3_stars");
                Buttons[i].enabled = true;
            }
        }

        int counterAllStars = 0;

        for(int j = 0; j < StarsCounters.Length; j++) { counterAllStars += StarsCounters[j]; }

        TextCounterAllStars.text = counterAllStars + "/72";
    }

    public void SpawnLevel()
    {
        CanvasChooseLevel.SetActive(false);
        CanvasLevel.SetActive(true);
        adsManager.ShowFullscreenAd();
        gameWorld.ClearWorld();
        gameWorld.GenerateCurrentLevel();
    }
}
