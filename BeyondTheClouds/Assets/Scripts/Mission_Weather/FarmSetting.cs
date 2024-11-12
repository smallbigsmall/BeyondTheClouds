using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmSetting : MissionSettingWithQuest
{
    private int cropCount = 0;
    [SerializeField] WeatherMissionManager _weatherMissionManager;
    private bool alreadyCleared = false;
    [SerializeField] GameObject Shadow;

    public void FarmCropSettingBlue() {
        cropCount = gameObject.transform.GetChild(0).childCount;

        for (int i = 0; i < cropCount; i++) {
            transform.GetChild(0).GetChild(i).gameObject.GetComponent<CropSetting>().CropColorSetting("#55D9FF", false);
        }

        cropCount = 0;
        Shadow.SetActive(true);
    }

    public void FarmCropSettingYellow()
    {
        cropCount = gameObject.transform.GetChild(0).childCount;

        for (int i = 0; i < cropCount; i++)
        {
            transform.GetChild(0).GetChild(i).gameObject.GetComponent<CropSetting>().CropColorSetting("#CF9700", true);
        }
    }

    public void countCropComplete() {
        cropCount -= 1;
        if (cropCount == 0 && !alreadyCleared) {
            //UI º¯°æ
            _weatherMissionManager.MissionComplete();
            CompleteQuestUI();
            alreadyCleared = true;
            for (int i = 0; i < gameObject.transform.GetChild(0).childCount; i++)
            {
                transform.GetChild(0).GetChild(i).gameObject.GetComponent<CropSetting>().MissionCleared();
            }
        }
    }

    public void countCropOverwatering() {
        cropCount += 1;
    }

    public void ChildHeatStrokeSetting()
    {
        transform.GetChild(1).gameObject.GetComponent<HeatStroke>().MakeNPCHeatStroke(_weatherMissionManager);
    }
}
