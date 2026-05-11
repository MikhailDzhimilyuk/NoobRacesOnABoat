using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YG;

public class FinishController : MonoBehaviour
{
    [SerializeField] private Image[] ImageStars;
    [SerializeField] private TextMeshProUGUI[] TextStars;
    [SerializeField] private TextMeshProUGUI[] TextStarsResult;
    [SerializeField] private GameObject ButtonLose;
    [SerializeField] private GameObject ButtonWin;
    [SerializeField] private ChooseLevelController ChooseLevelController;
    [SerializeField] private Saving_Manager_YSDK saving_Manager_YSDK;
    [SerializeField] private ShopController shopController;
    [SerializeField] private AdsManager adsManager;
    [SerializeField] private PauseController pauseController;
    [SerializeField] private AudioSource audioWin;
    [SerializeField] private AudioSource audioLose;
    [SerializeField] private SettingsController settingsController;
    private float audioBackgroundVolume;
    private void OnEnable()
    {
        int counterResultStars = 0;
        pauseController.ShowCursor();
        TextStars[0].text = "> 0";
        TextStars[1].text = "> 0";
        TextStars[2].text = Levels.coinsOfLevels[GameWorld.currentLevel].ToString();

        if (MyPlayerController.heartsCounter > 0) { ImageStars[0].sprite = Resources.Load<Sprite>("Images/full_star"); counterResultStars += 1; } else { ImageStars[0].sprite = Resources.Load<Sprite>("Images/empty_star"); }

        if (MyPlayerController.timeLevelCounter > 0) { ImageStars[1].sprite = Resources.Load<Sprite>("Images/full_star"); counterResultStars += 1; } else { ImageStars[1].sprite = Resources.Load<Sprite>("Images/empty_star"); }

        if (MyPlayerController.coinsLevel >= Levels.coinsOfLevels[GameWorld.currentLevel]) { ImageStars[2].sprite = Resources.Load<Sprite>("Images/full_star"); counterResultStars += 1; } else { ImageStars[2].sprite = Resources.Load<Sprite>("Images/empty_star"); }

        TextStarsResult[0].text = MyPlayerController.heartsCounter.ToString();
        TextStarsResult[1].text = MyPlayerController.timeLevelCounter.ToString();
        TextStarsResult[2].text = MyPlayerController.coinsLevel.ToString();

        bool isWin = (counterResultStars > 0) ? true : false;

        ButtonLose.SetActive(!isWin);
        ButtonWin.SetActive(isWin);

        if (ChooseLevelController.GetStarsCounter(GameWorld.currentLevel) < counterResultStars)
        {
            ChooseLevelController.SetStarCounters(GameWorld.currentLevel, counterResultStars);
            YandexGame.NewLeaderboardScores("LeaderBoardStarsLevels", ChooseLevelController.GetSummaryStarsCounter());
        }

        if (counterResultStars > 0)
        {
            audioWin.volume = SettingsController.ResultVolumeMusic;
            audioWin.Play();
        }

        else
        {
            audioLose.volume = SettingsController.ResultVolumeMusic;
            audioLose.Play();
        }
        
        shopController.ChangeBalance(MyPlayerController.coinsLevel);
        ChooseLevelController.UpdateInfoLevels();
        saving_Manager_YSDK.SaveData();

        adsManager.SetCanShowFullscreenAd();

        audioBackgroundVolume = settingsController.GetAudioBackgroundVolume();
        settingsController.SetAudioBackgroundVolume(0);
    }

    private void OnDisable()
    {
        settingsController.SetAudioBackgroundVolume(audioBackgroundVolume);
    }
}
