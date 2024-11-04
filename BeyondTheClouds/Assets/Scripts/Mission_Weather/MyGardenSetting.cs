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
            //UI ����
            _weatherMissionManager.MissionComplete();
            CompleteQuestUI();
            //������� ���� ���� �� �ְ� ���� �������ѳ����� �����
            //���� ���� �� �ְ� �ҰŸ� FlowerSetting�� MissionCompleted ���� ����
            //�̼� �Ϸ� ���� ��� �� �ȹٲ�� �ؾ���
            //������Ű���� ������ �ǵ������...�ٵ� �̰� �� ���ŷο� ��? �� �� ��
            //�ȹٲ�� �ϴ°� ������...
        }
    }

    public void countFlowerDrought()
    {
        flowerCount += 1;
    }
}
