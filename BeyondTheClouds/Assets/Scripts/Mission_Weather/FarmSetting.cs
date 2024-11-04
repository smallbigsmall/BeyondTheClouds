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
            //UI ����
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
     * ���� ���� ��ü�� �����߸� �̼� �������� ġ�µ�
     * ���� ������ �Ǵ°ɷ� ���� ���...
     * �̼��� �̹� ���� �ƴµ� ������ ���ְų� �ٽ� ���� ��� �� ���� �� ����...
     * 1. ���� �����ϱ�
     * 2. �۹� ��ȭ ���ϰ� �ϱ�
     * �� �ΰ��� �߿��� �ؾ��ҰͰ����� ��ν�
     */
}
