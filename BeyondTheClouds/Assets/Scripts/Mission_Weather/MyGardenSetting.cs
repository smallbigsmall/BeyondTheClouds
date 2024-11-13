using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGardenSetting : MissionSettingWithQuest
{
    private int flowerCount = 0;
    [SerializeField] WeatherMissionManager _weatherMissionManager;
    private bool alreadyCleared = false;
    [SerializeField] GameObject Shadow;

    public void MyGardenSettingSprout() {
        flowerCount = gameObject.transform.GetChild(0).childCount;

        for (int i = 0; i < flowerCount; i++)
        {
            gameObject.transform.GetChild(0).GetChild(i).gameObject.GetComponent<FlowerSetting>().InitToSprout();
        }
    }

    public void FlowerSettingBlue()
    {
        flowerCount = gameObject.transform.GetChild(0).childCount;

        for (int i = 0; i < flowerCount; i++)
        {
            gameObject.transform.GetChild(0).GetChild(i).gameObject.GetComponent<FlowerSetting>().FlowerColorSetting("#55D9FF", false);
        }

        flowerCount = 0;
        Shadow.SetActive(true);
    }

    public void FlowerSettingYellow()
    {
        flowerCount = gameObject.transform.GetChild(0).childCount;

        for (int i = 0; i < flowerCount; i++)
        {
            gameObject.transform.GetChild(0).GetChild(i).gameObject.GetComponent<FlowerSetting>().FlowerColorSetting("#CF9700", true);
        }
    }

    public void countFlowerComplete()
    {
        flowerCount -= 1;
        if (flowerCount == 0 && !alreadyCleared)
        {
            //UI º¯°æ
            _weatherMissionManager.MissionComplete();
            CompleteQuestUI();
            alreadyCleared = true;
            for (int i = 0; i < gameObject.transform.GetChild(0).childCount; i++)
            {
                transform.GetChild(0).GetChild(i).gameObject.GetComponent<FlowerSetting>().MissionCleared();
            }
        }
    }

    public void countFlowerOverwatering()
    {
        flowerCount += 1;
    }
}
