using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameWorld : MonoBehaviour
{
    [SerializeField] private GameObject Water;
    [SerializeField] private GameObject[] Glasses;
    [SerializeField] private GameObject[] Modules;
    [SerializeField] private GameObject DownModule30;
    [SerializeField] private GameObject GlassModule30;
    [SerializeField] private GameObject[] SplitterOpen;
    [SerializeField] private GameObject[] SplitterClose;
    [SerializeField] private GameObject[] Obstacles;
    [SerializeField] private GameObject Coin;
    [SerializeField] private GameObject Finish;
    [SerializeField] private GameObject Player;
    [SerializeField] private DayCycleController dayCycleController;
    [SerializeField] private GameObject LoadingPanel;
    [SerializeField] private TextMeshProUGUI LoaderText;
    [SerializeField] private GameObject PanelSettings;
    [SerializeField] private PauseController pauseController;
    [SerializeField] private GameObject CameraMain;
    [SerializeField] private AdsManager adsManager;
    private Color32[] colorsLoaderText = new Color32[] { new Color32(255, 5, 0, 255), new Color32(65, 255, 0, 255), new Color32(0, 88, 255, 255), new Color32(132, 0, 255, 255), new Color32(255, 186, 0, 255), new Color32(255, 255, 255, 255) };
    private MyPlayerController myPlayerController;
    public static int currentLevel = 0; // от 0 до 23
    private int[] platformCounters = { 0, 0 };
    private int[] indexLevelsCounters = { 0, 0 }; // на каком индексе массива с платформами находится
    private int[] StartSpawnXCoordinats = { 0, 0 };
    public static Vector3 Delta = new Vector3(0,0,0);
    private int indexSplitterOpen = 0;
    private int indexSplitterClose = 0;
    private bool isRightSpawn = false;
    // private float[] timesOfGeneration = new float[] {0.2f, }

    private List<GameObject> ListObjects = new List<GameObject>();

    private Dictionary<int, Vector2> CoordinatsObstacles = new Dictionary<int, Vector2>()
    {
        { 0, new Vector2(-11, 7.005f) },
        { 1, new Vector2(-9, 5.85f) },
        { 2, new Vector2(-7, 4.696f) },
        { 3, new Vector2(0, 0.6552f) },
        { 4, new Vector2(7, -3.387f) },
        { 5, new Vector2(9, -4.541f) },
        { 6, new Vector2(11, -5.695f) },
    };

    void Start()
    {
        myPlayerController = Player.GetComponent<MyPlayerController>();
        //CountMoneyCentralRow();
    }

    public void CountMoneyCentralRow()
    {
        string str = "";
        string str2 = "";
        string timeOfLevels = "";

        for (int i = 0; i < Levels.Levels_Central.Length; i++)
        {
            int num = 0;
            int numPlatforms = 0;

            for (int j = 0; j < Levels.Levels_Central[i].Length; j += 5)
            {
                int numInPlatforms = 0;

                for (int k = 0; k < Levels.CoinsTemplates[Levels.Levels_Central[i][j + 4]].Length; k++)
                {
                    if (Levels.CoinsTemplates[Levels.Levels_Central[i][j + 4]][k] != -1) numInPlatforms += 1;
                }

                numInPlatforms *= Levels.Levels_Central[i][j];
                num += numInPlatforms;
                numPlatforms += Levels.Levels_Central[i][j];
            }

            int newNum = num * 7 / 10 / 10 * 10; // необходимое число монет, для прохождения уровня;
            int newNumPlatforms = numPlatforms * 35 / 10 / 10 * 10; // время уровня
            //str += "Level: " + (i + 1).ToString() + " " + "Coins: " + (num).ToString() + "\n";
            str2 += (newNum).ToString() + ", ";//"Level: " + (i + 1).ToString() + " " + "Coins: " + (newNum).ToString() + "\n";
            timeOfLevels += (newNumPlatforms).ToString() + ", ";//"Level: " + (i + 1).ToString() + " " + "Platforms: " + (newNumPlatforms).ToString() + "\n";
        }

        Debug.Log(str);
        Debug.Log(str2);
        Debug.Log(timeOfLevels);
    }

    public void GenerateCurrentLevel()
    {
        StartCoroutine(StartGameSpawn(1f, Levels.Levels_Central, 0, 0));
        StartCoroutine(SetActiveObjects());
        StartCoroutine(ShowAndHideLoadingPanel());
    }

    public void ClearWorld()
    {
        CameraMain.transform.parent.rotation = Quaternion.Euler(0, 0, 0);
        CameraMain.transform.rotation = Quaternion.Euler(12, -90, 0);
        LoaderText.text = "5";
        LoaderText.color = colorsLoaderText[0];
        isRightSpawn = false;
        dayCycleController.SetTimeOfDay(0.5f);
        indexSplitterClose = 0;
        indexSplitterOpen = 0;
        if (transform.childCount > 0)
        {
            StopAllCoroutines();
            /* GameObject[] Childrens = new GameObject[transform.childCount];

             for(int i = 0; i < transform.childCount; i++)
             {
                 Childrens[i] = transform.GetChild(i).gameObject;
             }

             for (int i = 0; i < transform.childCount; i++)
             {
                 Destroy(Childrens[i]);
             }*/

            if (ListObjects.Count > 0)
            {
                for (int i = 0; i < ListObjects.Count; i++)
                {
                    Destroy(ListObjects[i]);
                }
            }

            ListObjects.Clear();

            platformCounters[0] = platformCounters[1] = indexLevelsCounters[0] = indexLevelsCounters[1] = StartSpawnXCoordinats[0] = StartSpawnXCoordinats[1] = 0;
            Delta.x = Delta.y = Delta.z = 0;
        }

        myPlayerController.ClearPlayer();
        Player.transform.position = new Vector3(32, 5 , 0);
        Player.transform.rotation = Quaternion.Euler(0, 0, 0);
        pauseController.HideCursor();
    }

    private IEnumerator StartGameSpawn(float time, int[][] LevelArray, int zOffset, int row) // i - левый (0) или правый ряд (1)
    {
        yield return new WaitForSecondsRealtime(time);

        GameObject module = null;

        int randomModule = (LevelArray[currentLevel][indexLevelsCounters[row] * 5 + 2] == -1) ? Random.Range(0, Modules.Length) : LevelArray[currentLevel][indexLevelsCounters[row] * 5 + 2];
        int randomGlasses = (LevelArray[currentLevel][indexLevelsCounters[row] * 5 + 3] == -1) ? Random.Range(0, Glasses.Length) : LevelArray[currentLevel][indexLevelsCounters[row] * 5 + 3];
        if (LevelArray[currentLevel][indexLevelsCounters[row] * 5 + 1] == 6)
        {
            indexSplitterClose = (indexSplitterClose == 0) ? 1 : 0;
            module = Instantiate(SplitterClose[indexSplitterClose], new Vector3(-(StartSpawnXCoordinats[row] + platformCounters[row] * 32) + Delta.x - 32, 0 + Delta.y, zOffset), Quaternion.Euler(0, 0, 0), transform);
            ListObjects.Add(module);
            Delta.x -= 32;
        }

        else if (LevelArray[currentLevel][indexLevelsCounters[row] * 5 + 1] == 5)
        {
            indexSplitterOpen = (indexSplitterOpen == 0) ? 1 : 0;
            module = Instantiate(SplitterOpen[indexSplitterOpen], new Vector3(-(StartSpawnXCoordinats[row] + platformCounters[row] * 32) + Delta.x, 0 + Delta.y, zOffset), Quaternion.Euler(0, 0, 0), transform);
            ListObjects.Add(module);
            Delta.x -= 32;
        }

        else if (LevelArray[currentLevel][indexLevelsCounters[row] * 5 + 1] == 4)
        {
            module = Instantiate(DownModule30, new Vector3(-(StartSpawnXCoordinats[row] + platformCounters[row] * 32) + Delta.x, -6.5f + Delta.y, zOffset), Quaternion.Euler(0, 180, -30), transform);
            ListObjects.Add(module);
            Delta.y -= 25;
            Delta.x += 6;
        }

        else if (LevelArray[currentLevel][indexLevelsCounters[row] * 5 + 1] == 3)
        {
            module = Instantiate(GlassModule30, new Vector3(-(StartSpawnXCoordinats[row] + platformCounters[row] * 32) + Delta.x, 0 + Delta.y, zOffset), Quaternion.Euler(0, 0, 30), transform);
            ListObjects.Add(module);
            if (platformCounters[row] == 0 && LevelArray[currentLevel][indexLevelsCounters[row] * 5 - 4] != 3)
            {
                if (module != null)
                {
                    BoxCollider boxCollider = module.AddComponent<BoxCollider>();

                    boxCollider.enabled = true;
                    boxCollider.center = new Vector3(16 - 16 * (LevelArray[currentLevel][indexLevelsCounters[row] * 5] + 10) + 17, 0, 0);
                    boxCollider.size = new Vector3(32 * (LevelArray[currentLevel][indexLevelsCounters[row] * 5] + 10) + 34, 3.961f, 14);

                    BoxCollider triggerLockRotate = module.AddComponent<BoxCollider>();
                    triggerLockRotate.center = new Vector3(-2, 4, 0);
                    triggerLockRotate.size = new Vector3(28, 12, 14);
                    triggerLockRotate.isTrigger = true;
                    module.name = "LockRotateModule";
                }
            }

            yield return new WaitForSecondsRealtime(0.005f);
            Instantiate(Glasses[randomGlasses], new Vector3(-(StartSpawnXCoordinats[row] + platformCounters[row] * 32) + Delta.x - 1.5f, 3 + Delta.y - 0.4f, zOffset), Quaternion.Euler(-60, -90, 90), module.transform);

            for (int i = 0; i < Levels.ObstaclesTemplates[LevelArray[currentLevel][indexLevelsCounters[row] * 5 + 4]].Length; i++)
            {
                int index = Levels.ObstaclesTemplates[LevelArray[currentLevel][indexLevelsCounters[row] * 5 + 4]][i];
                int indexCoin = Levels.CoinsTemplates[LevelArray[currentLevel][indexLevelsCounters[row] * 5 + 4]][i];

                if (index != -1)
                {
                    yield return new WaitForSecondsRealtime(0.005f);
                    Instantiate(Obstacles[index], new Vector3(-(StartSpawnXCoordinats[row] + platformCounters[row] * 32 + Obstacles[index].transform.localPosition.x + CoordinatsObstacles[i].x) + Delta.x, Obstacles[index].transform.localPosition.y + Delta.y + CoordinatsObstacles[i].y, Obstacles[index].transform.localPosition.z + zOffset), Quaternion.Euler(-60, -90, 90), module.transform);
                }

                if (indexCoin != -1)
                {
                    Instantiate(Obstacles[indexCoin], new Vector3(-(StartSpawnXCoordinats[row] + platformCounters[row] * 32 + Obstacles[indexCoin].transform.localPosition.x + CoordinatsObstacles[i].x) + Delta.x, Obstacles[indexCoin].transform.localPosition.y + Delta.y, Obstacles[indexCoin].transform.localPosition.z + zOffset), Quaternion.Euler(0, 0, 0), module.transform);
                }
            }

            if (platformCounters[row] >= LevelArray[currentLevel][indexLevelsCounters[row] * 5] - 1)
            {
                if (LevelArray[currentLevel][indexLevelsCounters[row] * 5 + 6] == 3) { Delta.y -= 8; }

                else
                {
                    BoxCollider triggerLockRotate = module.AddComponent<BoxCollider>();
                    triggerLockRotate.center = new Vector3(-18, 4, 0);
                    triggerLockRotate.size = new Vector3(22, 12, 14);
                    triggerLockRotate.isTrigger = true;
                    module.name = "UnLockRotateModule";
                }
            }

            Delta.y -= 16;
            Delta.x += 4.2871870788979f;
        }

        else if (LevelArray[currentLevel][indexLevelsCounters[row] * 5 + 1] > 0)
        {
            module = Instantiate(Modules[randomModule], new Vector3(-(StartSpawnXCoordinats[row] + platformCounters[row] * 32) + Delta.x, 0 + Delta.y, zOffset), Quaternion.Euler(0, 0, 0), transform);
            ListObjects.Add(module);
            yield return new WaitForSecondsRealtime(0.005f);
            Instantiate(Glasses[randomGlasses], new Vector3(-(StartSpawnXCoordinats[row] + platformCounters[row] * 32) + Delta.x, 3 + Delta.y, zOffset), Quaternion.Euler(-90, 0, 0), module.transform);

            for (int i = 0; i < Levels.ObstaclesTemplates[LevelArray[currentLevel][indexLevelsCounters[row] * 5 + 4]].Length; i++)
            {
                int index = Levels.ObstaclesTemplates[LevelArray[currentLevel][indexLevelsCounters[row] * 5 + 4]][i];
                int indexCoin = Levels.CoinsTemplates[LevelArray[currentLevel][indexLevelsCounters[row] * 5 + 4]][i];
                if (index != -1)
                {
                    yield return new WaitForSecondsRealtime(0.005f);
                    Instantiate(Obstacles[index], new Vector3(-(StartSpawnXCoordinats[row] + platformCounters[row] * 32 + Obstacles[index].transform.localPosition.x + CoordinatsObstacles[i].x) + Delta.x, Obstacles[index].transform.localPosition.y + Delta.y, Obstacles[index].transform.localPosition.z + zOffset), Quaternion.Euler(-90, 0, 0), module.transform);
                }

                if (indexCoin != -1)
                {
                    yield return new WaitForSecondsRealtime(0.005f);
                    Instantiate(Obstacles[indexCoin], new Vector3(-(StartSpawnXCoordinats[row] + platformCounters[row] * 32 + Obstacles[indexCoin].transform.localPosition.x + CoordinatsObstacles[i].x) + Delta.x, Obstacles[indexCoin].transform.localPosition.y + Delta.y, Obstacles[indexCoin].transform.localPosition.z + zOffset), Quaternion.Euler(0, 0, 0), module.transform);
                }
            }
        }


        if (platformCounters[row] == 0 && LevelArray[currentLevel][indexLevelsCounters[row] * 5 + 1] != 4)
        {
            if (module != null)
            {
                BoxCollider boxCollider = module.AddComponent<BoxCollider>();

                boxCollider.center = new Vector3(16 - 16 * LevelArray[currentLevel][indexLevelsCounters[row] * 5], 0, 0);
                boxCollider.size = new Vector3(32 * LevelArray[currentLevel][indexLevelsCounters[row] * 5], 3.961f, 14);
            }
        }

        platformCounters[row] += 1;
        
        if (platformCounters[row] > LevelArray[currentLevel][indexLevelsCounters[row] * 5] - 1) 
        {
            if (LevelArray[currentLevel][indexLevelsCounters[row] * 5 + 1] == 3)
            {
                Delta.y += 8; // чуток поднимаю уровень чтобы сошлось после спуска
            }

            StartSpawnXCoordinats[row] += (platformCounters[row]) * 32;
            indexLevelsCounters[row] += 1;
            platformCounters[row] = 0;
        }

        if (indexLevelsCounters[row] < LevelArray[currentLevel].Length / 5)
        {
            /*if (row == 0)
            {
                StartCoroutine(StartGameSpawn(0.02125f, Levels.Levels_Right, 14, 1));
                StartCoroutine(StartGameSpawn(0.0375f, LevelArray, zOffset, row));
            }*/
            isRightSpawn = !isRightSpawn;

            if (isRightSpawn) { StartCoroutine(StartGameSpawn(0.005f, Levels.Levels_Right, 14, 1)); }
            else { StartCoroutine(StartGameSpawn(0.005f, Levels.Levels_Central, 0, 0)); }
        }

        else
        {
            GameObject moduleFinish = Instantiate(Finish, new Vector3(-(StartSpawnXCoordinats[row] + platformCounters[row] * 32) + Delta.x, 0 + Delta.y, zOffset), Quaternion.Euler(0, 0, 0), transform);
            ListObjects.Add(moduleFinish);
        }
    }


    private IEnumerator SetActiveObjects()
    {
        yield return new WaitForSecondsRealtime(0);

        if (ListObjects.Count > 0)
        {
            for (int i = 0; i < ListObjects.Count; i++)
            {
                yield return new WaitForSecondsRealtime(0.02f);
                float deltaX = ListObjects[i].transform.position.x - MyPlayerController.PlayerLocalPosition.x;
                if (deltaX > 200 || deltaX < -200) ListObjects[i].SetActive(false);
                else if (!ListObjects[i].activeInHierarchy) ListObjects[i].SetActive(true);
            }
        }


        StartCoroutine(SetActiveObjects());
    }

    private IEnumerator ShowAndHideLoadingPanel()
    {
        LoadingPanel.SetActive(true);

        if (adsManager.canShowFullscreenAd == true)
        {
            yield return new WaitUntil(() => adsManager.canShowFullscreenAd == false);
        }

        yield return new WaitForSecondsRealtime(0.1f);
        PauseController.SetPause();
        int counter = 5;

        for(int i = 0; i < 6; i++)
        {
            LoaderText.color = colorsLoaderText[i];
            LoaderText.text = counter.ToString();
            yield return new WaitForSecondsRealtime(1.2f);
            counter -= 1;
        }
        CameraMain.transform.parent.rotation = Quaternion.Euler(0, 0, 0);
        CameraMain.transform.rotation = Quaternion.Euler(12, -90, 0);
        myPlayerController.TickTack();
        LoadingPanel.SetActive(false);
        if (!PanelSettings.activeInHierarchy) { PauseController.SetUnpause(); }
    }
}
    