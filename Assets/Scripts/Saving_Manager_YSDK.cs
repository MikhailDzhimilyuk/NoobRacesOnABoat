using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
public class Saving_Manager_YSDK : MonoBehaviour
{
    [SerializeField] private ShopController shopController;
    [SerializeField] private ChooseLevelController chooseLevelController;
    private bool canSave = true;
    private void OnEnable() => YandexGame.GetDataEvent += GetLoad;

    // Отписываемся от события GetDataEvent в OnDisable
    private void OnDisable() => YandexGame.GetDataEvent -= GetLoad;

    private void Start()
    {
        if (YandexGame.SDKEnabled == true)
        {

            // Если запустился, то выполняем Ваш метод для загрузки
            //YandexGame.ResetSaveProgress(); // эта строка сбрасывает сохранения
            GetLoad();
            chooseLevelController.StartGame();
            shopController.StartGame();
            // Если плагин еще не прогрузился, то метод не выполнится в методе Start,
            // но он запустится при вызове события GetDataEvent, после прогрузки плагина
        }
    }

    public void SaveData()
    {
        if (canSave) 
        {
            for (int i = 0; i < YandexGame.savesData.StarsCounters.Length; i++) { YandexGame.savesData.StarsCounters[i] = chooseLevelController.GetStarsCounter(i); }

            for(int j = 0; j < YandexGame.savesData.pricesBoats.Length; j++) { YandexGame.savesData.pricesBoats[j] = shopController.GetData("pricesBoats", j); }

            for(int k = 0; k < YandexGame.savesData.pricesPlayers.Length; k++) { YandexGame.savesData.pricesPlayers[k] = shopController.GetData("pricesPlayers", k); }

            YandexGame.savesData.balance = shopController.GetData("balance");
            YandexGame.savesData.choosedBoat = shopController.GetData("choosedBoat");
            YandexGame.savesData.choosedPlayer = shopController.GetData("choosedPlayer");

            YandexGame.SaveProgress(); 
            canSave = false;

            StopAllCoroutines();

            StartCoroutine(SetCanSave());
             
            Debug.Log("Game was saved on Yandex server");
        }

        else
        {
            StartCoroutine(DelayedSave());
        }
    }
    // Start is called before the first frame update
    public void GetLoad()
    {
        shopController.GetLoad(
            YandexGame.savesData.choosedBoat, 
            YandexGame.savesData.choosedPlayer,
            YandexGame.savesData.balance,
            YandexGame.savesData.pricesBoats,
            YandexGame.savesData.pricesPlayers
        );
        shopController.StartGame();

        chooseLevelController.GetLoad(YandexGame.savesData.StarsCounters);
        chooseLevelController.StartGame();
        YandexGame.FullscreenShow();
    }

    private IEnumerator SetCanSave() 
    {
        yield return new WaitForSecondsRealtime(3);
        canSave = true;
    }
    private IEnumerator DelayedSave()
    {
        yield return new WaitForSecondsRealtime(3.1f);
        SaveData();
    }
}
