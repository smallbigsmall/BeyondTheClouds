using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGardenSetting : MissionSettingWithQuest
{
    private int flowerCount = 0;
    [SerializeField] WeatherMissionManager _weatherMissionManager;

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
        if (flowerCount == 0)
        {
            //UI 변경
            _weatherMissionManager.MissionComplete();
            CompleteQuestUI();
            //만들어진 구름 없앨 수 있게 할지 고정시켜놓을지 고민중
            //만약 없앨 수 있게 할거면 FlowerSetting에 MissionCompleted 변수 만들어서
            //미션 완료 됐을 경우 색 안바뀌게 해야함
            //고정시키려면 구름쪽 건드려야함...근데 이게 더 번거로울 듯? 걍 꽃 색
            //안바뀌게 하는게 나을듯...
        }
    }

    public void countFlowerDrought()
    {
        flowerCount += 1;
    }
}
