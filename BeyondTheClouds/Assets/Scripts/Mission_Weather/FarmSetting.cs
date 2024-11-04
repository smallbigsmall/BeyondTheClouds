using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmSetting : MissionSettingWithQuest
{
    private int cropCount = 0;
    [SerializeField] WeatherMissionManager _weatherMissionManager;

    public void FarmCropSettingBlue() {
        cropCount = gameObject.transform.GetChild(0).childCount;

        for (int i = 0; i < cropCount; i++) {
            transform.GetChild(0).GetChild(i).gameObject.GetComponent<CropSetting>().CropColorSetting("#55D9FF", false);
        }
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
        if (cropCount == 0) {
            //UI 변경
            _weatherMissionManager.MissionComplete();
            CompleteQuestUI();
        }
    }

    public void countCropDrought() {
        cropCount += 1;
    }

    public void ChildHeatStrokeSetting()
    {
        transform.GetChild(1).gameObject.GetComponent<HeatStroke>().MakeNPCHeatStroke(_weatherMissionManager);
    }

    /*
     * 현재 영역 전체를 가려야만 미션 성공으로 치는데
     * 각각 가려도 되는걸로 할지 고민...
     * 미션이 이미 성공 됐는데 구름을 없애거나 다시 만들 경우 좀 꼬일 수 있음...
     * 1. 구름 고정하기
     * 2. 작물 변화 못하게 하기
     * 이 두가지 중에서 해야할것같은데 고민스
     */
}
