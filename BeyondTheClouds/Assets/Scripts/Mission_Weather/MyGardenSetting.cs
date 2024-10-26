using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGardenSetting : MonoBehaviour
{
    private int flowerCount = 0;
    [SerializeField] WeatherMissionManager _weatherMissionManager;

    public void MyGardenSettingSprout() {
        flowerCount = gameObject.transform.childCount;

        for (int i = 0; i < flowerCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.GetComponent<FlowerSetting>().InitToSprout();
        }
    }

    public void FlowerSettingBlue()
    {
        flowerCount = gameObject.transform.childCount;

        for (int i = 0; i < flowerCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.GetComponent<FlowerSetting>().FlowerColorSetting("#55D9FF");
        }
    }

    public void FlowerSettingYellow()
    {
        flowerCount = gameObject.transform.childCount;

        for (int i = 0; i < flowerCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.GetComponent<FlowerSetting>().FlowerColorSetting("#CF9700");
        }
    }

    public void countFlowerComplete()
    {
        flowerCount -= 1;
        if (flowerCount == 0)
        {
            //UI º¯°æ
            _weatherMissionManager.MissionComplete();
        }
    }
}
