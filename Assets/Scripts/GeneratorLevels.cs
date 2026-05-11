using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorLevels : MonoBehaviour
{
    private float timeDeg0Platform = 3.5f;
    private float timeDeg30Platform = 3f;
    private float SummaryTimeLevel;

    public static int[] LevelCentralArr;
    public static int[] LevelRightArr;
    private string Level;

    private int[] moduleTypes = new int[] { 1, 2, 3, 4, 5, 6 };
    private int[] platformTypes = new int[] { 0, 1, 2, 3, 4, 5, 6 };
    private int[] glassTypes = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

    private int[] obstacleTypes = new int[] { 
        93, 92, 
        91, 90, 89, 88, 87, 86,   
        85, 84, 83, 82, 81, 80,
        79, 78, 
        77, 76, 75, 74, 73,
        72, 71, 70, 72, 71, 70,
        69, 68, 69, 68, 69, 68,
        67, 66, 67, 66, 67, 66,
        13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1,
        26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14,
        39, 38, 37, 36, 35, 34, 33, 32, 31, 30, 29, 28, 27,
        52, 51, 50, 49, 48, 47, 46, 45, 44, 43, 42, 41, 40,
        65, 64, 63, 62, 61, 60, 59, 58, 57, 56, 55, 54, 53,
    };
    private int[] obstacleChances = new int[] { 
        15, 15, 15,
        20,
        20,
        15, 15, 15,
        15, 15, 15,
        30, 30,
        30, 30,
        60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60,
        100, 100, 100, 100, 100,
    };

    void Start()
    {
        GenerateCentralLevelModules(moduleTypes, 250, 4, 200);
        GenerateCentralLevel(platformTypes, obstacleTypes, obstacleChances, new Vector2Int(1, 3), new Vector2Int(1, 3));
        GenerateRightLevel(LevelCentralArr, obstacleTypes, obstacleChances);
        Debug.Log(ShowLevel(LevelCentralArr));
        Debug.Log(ShowLevel(LevelRightArr));
    }

    private void GenerateCentralLevelModules(int[] ModuleTypes, int levelLength, int maxLength30DegPlatform = 3, int chance30Deg = 100)
    {
        LevelCentralArr = new int[levelLength];

        for (int i = 0; i < LevelCentralArr.Length; i += 5)
        {
            LevelCentralArr[i + 1] = 1;
        }

        for (int i = 10; i < LevelCentralArr.Length - 45; i += 5)
        {
            int chance = Random.Range(0, chance30Deg);

            if (chance < 20 && LevelCentralArr[i + 1] == 1 && LevelCentralArr[i - 4] == 1)
            {
                LevelCentralArr[i + 1] = 4;
                LevelCentralArr[i + 6] = 3;
                
                if (chance < 18) { LevelCentralArr[i + 11] = 3; }

                if (chance < 16 && maxLength30DegPlatform > 3) { LevelCentralArr[i + 16] = 3; }

                if (chance < 14 && maxLength30DegPlatform > 4) { LevelCentralArr[i + 21] = 3; }

                if (chance < 12 && maxLength30DegPlatform > 5) { LevelCentralArr[i + 26] = 3; }

                if (chance < 10 && maxLength30DegPlatform > 6) { LevelCentralArr[i + 31] = 3; }
            }
        }

        for (int i = 25; i < LevelCentralArr.Length - 45; i += 5)
        {
            if (LevelCentralArr[i - 9] == 1 && LevelCentralArr[i - 4] == 1 && LevelCentralArr[i + 1] == 1 && LevelCentralArr[i + 6] == 1 && LevelCentralArr[i + 11] == 1)
            {
                int chance = Random.Range(0, 100);

                if (chance > 50) { if (LevelCentralArr[i - 14] == 1 && LevelCentralArr[i - 19] == 1) { LevelCentralArr[i - 14] = 5; LevelCentralArr[i + 11] = 6; } }
                else if (chance > 25) { if (LevelCentralArr[i - 14] == 1) { LevelCentralArr[i - 9] = 5; LevelCentralArr[i + 11] = 6; } }
                else if (chance > 0) { LevelCentralArr[i - 4] = 5; LevelCentralArr[i + 11] = 6; }
            }
        }
    }

    private void GenerateCentralLevel( int[] PlatformTypes, int[] ObstacleTypes, int[] ObstacleChances, Vector2Int lengthRange1, Vector2Int lengthRange2)
    {
        int prevTypePlatform = -1;

        for (int i = 0; i < LevelCentralArr.Length; i += 5 )
        {
            if (LevelCentralArr[i + 1] == 1)
            {
                LevelCentralArr[i] = Random.Range(lengthRange1.x, lengthRange1.y + 1);
                LevelCentralArr[i + 2] = PlatformTypes[Random.Range(0, PlatformTypes.Length)];
                LevelCentralArr[i + 3] = -1;
                LevelCentralArr[i + 4] = GenerateObstacles(ObstacleTypes, ObstacleChances);
            }

            else if (LevelCentralArr[i + 1] == 3)
            {
                LevelCentralArr[i] = Random.Range(lengthRange2.x, lengthRange2.y + 1);
                LevelCentralArr[i + 2] = 0;
                LevelCentralArr[i + 3] = -1;
                LevelCentralArr[i + 4] = GenerateObstacles(ObstacleTypes, ObstacleChances);
            }

            else if (LevelCentralArr[i + 1] > 0)
            {
                LevelCentralArr[i] = 1;
                LevelCentralArr[i + 2] = 0;
                LevelCentralArr[i + 3] = 0;
                LevelCentralArr[i + 4] = 0;
            }
        }
    }

    private void GenerateRightLevel(int[] levelCentral ,int[] ObstacleTypes, int[] ObstacleChances)
    {
        LevelRightArr = new int[levelCentral.Length];

        for (int i = 0; i < levelCentral.Length; i += 5)
        {
            LevelRightArr[i] = levelCentral[i];
        } 

        for (int i = 5; i < levelCentral.Length - 45; i += 5)
        {
            if (levelCentral[i + 1] == 5)
            {
                int m = 5;
               
                while (levelCentral[i + 1 + m] != 6)
                {
                    LevelRightArr[i + m + 1] = 1;
                    LevelRightArr[i + m + 2] = -1;
                    LevelRightArr[i + m + 3] = -1;
                    LevelRightArr[i + m + 4] = GenerateObstacles(ObstacleTypes, ObstacleChances); ;

                    m += 5;
                }
            }
        }
    }

    private int GenerateObstacles(int[] ObstacleTypes, int[] ObstacleChances)
    {
        int num;
        int randomNum = Random.Range(0, 101);
        int[] emptyArr = new int[] { 0, 0, 94, 95, 96, 97, 98 };

        if (randomNum >= 15)
        {
            num = ObstacleTypes[Random.Range(0, ObstacleTypes.Length)];
        }

        else { num = emptyArr[Random.Range(0, emptyArr.Length)]; }

        return num;
    }

    private string ShowLevel(int[] arrLevel)
    {
        string str = "new int[]\n{\n";

        for (int i = 0; i < arrLevel.Length; i++)
        {
            if (i % 5 == 0) { str += "   "; }

            str += " " + arrLevel[i] + ",";

            if ((i + 1) % 5 == 0) { str += "\n"; }
        }

        str += "},";

        return str;
    }
}
