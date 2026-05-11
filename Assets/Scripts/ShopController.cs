using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopController : MonoCache
{
    [SerializeField] private GameObject MenuBoats;
    [SerializeField] private GameObject MenuPlayers;

    [SerializeField] private GameObject[] Boats = new GameObject[5];
    [SerializeField] private GameObject[] Players = new GameObject[4];
    [SerializeField] private GameObject ButtonBuy;
    [SerializeField] private GameObject ButtonChoose;
    [SerializeField] private Image ImageButtonChoose;
    [SerializeField] private TextMeshProUGUI TextPrice;
    [SerializeField] private GameObject[] PlayersInGame;
    [SerializeField] private GameObject[] BoatsInGame;
    [SerializeField] private TextMeshProUGUI TextBalanceInGame;
    [SerializeField] private TextMeshProUGUI TextBalanceInGame2;
    [SerializeField] private Saving_Manager_YSDK saving_Manager_YSDK;
    [SerializeField] private ChooseLevelController chooseLevelController;
    [SerializeField] private GameObject EtcStars;

    private bool ChoosedIsBoats = false;
    private int choosedBoat = 0;
    private int choosedPlayer = 0;

    private int currentBoat = 0;
    private int currentPlayer = 0;
    private int balance = 1000;

    private int[] pricesBoats = new int[] {0, 500, 700, 1000, 1500, 2500 };
    private int[] pricesPlayers = new int[] {0, 1500, 1500, 2000 };

    public void StartGame()
    {
        TextBalanceInGame.text = balance.ToString();
        TextBalanceInGame2.text = balance.ToString();
        HideAllObjects(BoatsInGame);
        HideAllObjects(PlayersInGame);
        ShowObject(BoatsInGame, choosedBoat);
        ShowObject(PlayersInGame, choosedPlayer);
    }

    public void ChangeBalance(int num)
    {
        balance += num;
        TextBalanceInGame.text = balance.ToString();
        TextBalanceInGame2.text = balance.ToString();
    }

    public void ExitThisLevel()
    {
        ChangeBalance(MyPlayerController.coinsLevel);
        chooseLevelController.UpdateInfoLevels();
        saving_Manager_YSDK.SaveData();
    }

    public int GetData(string nameValue, int index = 0)
    {
        switch (nameValue)
        {
            case "balance": return balance;
            case "choosedBoat": return choosedBoat;
            case "choosedPlayer": return choosedPlayer;
            case "pricesBoats": return pricesBoats[index];
            case "pricesPlayers": return pricesPlayers[index];

            default: return 0;
        }
    }

    public void GetLoad(int choosedBoatYG, int choosedPlayerYG, int balanceYG, int[] pricesBoatsYG, int[] pricesPlayersYG)
    {
        choosedBoat = choosedBoatYG;
        choosedPlayer = choosedPlayerYG;
        balance = balanceYG;
        
        if (choosedPlayer == 3) { EtcStars.transform.localPosition = new Vector3(0.00637f, 0.02172f, 0); }

        for(int i = 0; i < pricesBoatsYG.Length; i++) { pricesBoats[i] = pricesBoatsYG[i]; }

        for (int j = 0; j < pricesPlayersYG.Length; j++) { pricesPlayers[j] = pricesPlayersYG[j]; }
    }

    public override void OnFixedTick()
    {
        if (MenuBoats.activeInHierarchy)
        {
            float newXAngles = (MenuBoats.transform.localEulerAngles.y - 180 > 0) ? 360 - MenuBoats.transform.localEulerAngles.y : MenuBoats.transform.localEulerAngles.y;

            MenuBoats.transform.localEulerAngles = new Vector3(-36 + newXAngles / 2.5f, MenuBoats.transform.localEulerAngles.y - 50 * Time.fixedDeltaTime, MenuBoats.transform.localEulerAngles.z);
        }
        else if (MenuPlayers.activeInHierarchy)
        {
            float newXAngles = (MenuPlayers.transform.localEulerAngles.y - 180 > 0) ? 360 - MenuPlayers.transform.localEulerAngles.y : MenuPlayers.transform.localEulerAngles.y;

            MenuPlayers.transform.localEulerAngles = new Vector3(0, MenuPlayers.transform.localEulerAngles.y - 50 * Time.fixedDeltaTime, -5 + newXAngles / 18f);
        }
    }

    public void ChooseBoats()
    {
        MenuBoats.transform.localEulerAngles = new Vector3(-36, 0, 0);
        ChoosedIsBoats = true;
        HideAllObjects(Boats);
        HideAllObjects(Players);
        ShowObject(Boats, currentBoat);

        if (pricesBoats[currentBoat] == 0)
        {
            ButtonChoose.SetActive(true);
            ImageButtonChoose.sprite = (currentBoat == choosedBoat) ? Resources.Load<Sprite>("Images/ok") : Resources.Load<Sprite>("Images/notok");
            ButtonBuy.SetActive(false);
        }

        else
        {
            ButtonChoose.SetActive(false);
            ButtonBuy.SetActive(true);
            TextPrice.text = pricesBoats[currentBoat].ToString();
        }
    }

    public void ChoosePlayers()
    {
        MenuPlayers.transform.localEulerAngles = new Vector3(0, 90, -5);
        ChoosedIsBoats = false;
        HideAllObjects(Boats);
        HideAllObjects(Players);
        ShowObject(Players, currentPlayer);

        if (pricesPlayers[currentPlayer] == 0)
        {
            ButtonChoose.SetActive(true);
            ImageButtonChoose.sprite = (currentPlayer == choosedPlayer) ? Resources.Load<Sprite>("Images/ok") : Resources.Load<Sprite>("Images/notok");
            ButtonBuy.SetActive(false);
        }

        else
        {
            ButtonChoose.SetActive(false);
            ButtonBuy.SetActive(true);
            TextPrice.text = pricesPlayers[currentPlayer].ToString();
        }

        if (choosedPlayer == 3) { EtcStars.transform.localPosition = new Vector3(0.00637f, 0.02172f, 0); } else
        {
            EtcStars.transform.localPosition = new Vector3(0.00837f, 0.01672f, 0);
        }
    }

    public void ShowNextGameObject()
    {
        if (ChoosedIsBoats)
        {
            currentBoat = (currentBoat == Boats.Length - 1) ? 0 : currentBoat + 1;
            ChooseBoats();
        }
        
        else
        {
            currentPlayer = (currentPlayer == Players.Length - 1) ? 0 : currentPlayer + 1;
            ChoosePlayers();
        }
    }

    public void ShowPrevGameObject()
    {
        if (ChoosedIsBoats)
        {
            currentBoat = (currentBoat == 0) ? Boats.Length - 1 : currentBoat - 1;
            ChooseBoats();
        }

        else
        {
            currentPlayer = (currentPlayer == 0) ? Players.Length - 1 : currentPlayer - 1;
            ChoosePlayers();
        }
    }

    public void ShowObject(GameObject[] arrayObjects, int currentIndex)
    {
        for (int i = 0; i < arrayObjects.Length; i++)
        {
            if (i == currentIndex) { arrayObjects[i].SetActive(true); return; }
        }
    }

    public void ChooseThis()
    {
        if (ChoosedIsBoats && choosedBoat != currentBoat)
        {
            choosedBoat = currentBoat;
            ChooseBoats();
            HideAllObjects(BoatsInGame);
            ShowObject(BoatsInGame, choosedBoat);
            saving_Manager_YSDK.SaveData();
        }

        else if (choosedPlayer != currentPlayer)
        {
            choosedPlayer = currentPlayer;
            ChoosePlayers();
            HideAllObjects(PlayersInGame);
            ShowObject(PlayersInGame, choosedPlayer);
            saving_Manager_YSDK.SaveData();
        }
    }

    public void BuyThis()
    {
        if (ChoosedIsBoats)
        {
            if (balance >= pricesBoats[currentBoat])
            {
                balance -= pricesBoats[currentBoat];
                pricesBoats[currentBoat] = 0;
                choosedBoat = currentBoat;
                ChooseBoats();
                HideAllObjects(BoatsInGame);
                ShowObject(BoatsInGame, choosedBoat);
                saving_Manager_YSDK.SaveData();
            }
        }

        else
        {
            if (balance >= pricesPlayers[currentPlayer])
            {
                balance -= pricesPlayers[currentPlayer];
                pricesPlayers[currentPlayer] = 0;
                choosedPlayer = currentPlayer;
                ChoosePlayers();
                HideAllObjects(PlayersInGame);
                ShowObject(PlayersInGame, choosedPlayer);
                saving_Manager_YSDK.SaveData();
            }
        }

        TextBalanceInGame.text = balance.ToString();
        TextBalanceInGame2.text = balance.ToString();
    }

    public void HideAllObjects(GameObject[] arrayObjects)
    {
        for (int i = 0; i < arrayObjects.Length; i++) { arrayObjects[i].SetActive(false); }
    }
}
