using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmSetting : MonoBehaviour
{
    public void FarmCropSettingBlue() {
        int childCount = gameObject.transform.childCount;

        for (int i = 0; i < childCount; i++) {
            gameObject.transform.GetChild(i).gameObject.GetComponent<CropSetting>().CropColorSetting("#55D9FF");
        }
    }

    public void FarmCropSettingYellow()
    {
        int childCount = gameObject.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.GetComponent<CropSetting>().CropColorSetting("#CF9700");
        }
    }
}
