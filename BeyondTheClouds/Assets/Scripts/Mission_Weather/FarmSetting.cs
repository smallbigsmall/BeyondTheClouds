using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmSetting : MonoBehaviour
{
    private int cropCount = 0;
    [SerializeField] WeatherMissionManager _weatherMissionManager;

    public void FarmCropSettingBlue() {
        cropCount = gameObject.transform.childCount;

        for (int i = 0; i < cropCount; i++) {
            gameObject.transform.GetChild(i).gameObject.GetComponent<CropSetting>().CropColorSetting("#55D9FF");
        }
    }

    public void FarmCropSettingYellow()
    {
        cropCount = gameObject.transform.childCount;

        for (int i = 0; i < cropCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.GetComponent<CropSetting>().CropColorSetting("#CF9700");
        }
    }

    public void countCropComplete() {
        cropCount -= 1;
        if (cropCount == 0) {
            //UI º¯°æ
            _weatherMissionManager.MissionComplete();
        }
    }
}
