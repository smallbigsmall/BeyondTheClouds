using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeatherMissionManager : MonoBehaviour
{
    //�̺�Ʈ1(1����)
    //  �̺�Ʈ����1
    //  �̺�Ʈ ����
    //  �̺�Ʈ ���
    //
    //  �̺�Ʈ����2
    //  �̺�Ʈ ����
    //  �̺�Ʈ ���

    public enum MissionType { None, drought, overwatering, heatstroke, fire, Random};
    public enum MissionLocation { None, myGarden, farm2, farm3, waterfall, forest, mountain, mine, Random };

    [Serializable]
    public class MissionInform { //�̼� ���� ����
        
        public MissionType Mission_Type;
        public MissionLocation Location;
    }

    [Serializable]
    public class MissionListOfDay { //�Ϸ�ġ �̼� ����Ʈ
        public List<MissionInform> missionList = new List<MissionInform>();
    }

    public int currentDay = 0; //for test
    public List<MissionListOfDay> missionListOfDay = new List<MissionListOfDay>(); //��� day�� �̼�

    [SerializeField] GameObject MyGarden, Farm2, Farm3, Waterfall, Forest, Mountain, Mine;
    [SerializeField] GameObject MissionUIPrefab; //UI ���� �����ϴ� ��ũ��Ʈ ���̱�, UI ����� �̼� ��ũ��Ʈ�� �Ѱ��ֱ�

    private int todayMissionCount = -1;

    void Start()
    {
        StartMissoinSetting();
    }

    void StartMissoinSetting() {
        List<MissionInform> todayMission = missionListOfDay[currentDay].missionList;
        todayMissionCount = todayMission.Count;

        for (int i = 0; i < todayMissionCount; i++) {
            switch (todayMission[i].Mission_Type) {
                case MissionType.drought:
                    Drought(todayMission[i].Location);
                    break;
                case MissionType.overwatering:
                    Overwatering(todayMission[i].Location);
                    break;
                case MissionType.heatstroke:
                    Heatstroke(todayMission[i].Location);
                    break;
                case MissionType.fire:
                    Fire(todayMission[i].Location);
                    break;
                default:
                    break;
            }
        }
    }

    void Drought(MissionLocation ML) {
        if (ML == MissionLocation.farm2)
        {
            Farm2.GetComponent<FarmSetting>().FarmCropSettingYellow();
        }
        else if (ML == MissionLocation.farm3)
        {
            Farm3.GetComponent<FarmSetting>().FarmCropSettingYellow();
        }
        else if (ML == MissionLocation.myGarden)
        {
            if (currentDay == 1) {
                MyGarden.GetComponent<MyGardenSetting>().MyGardenSettingSprout();
            } 
            else{
                MyGarden.GetComponent<MyGardenSetting>().FlowerSettingYellow();
            }
        }
        else if (ML == MissionLocation.waterfall) {
            Waterfall.GetComponent<WaterfallMission>().missionSetting();
        }
    }

    void Overwatering(MissionLocation ML) {
        Debug.Log("Location: " + ML.ToString() + " Mission type is overwatering");
        if (ML == MissionLocation.farm2)
        {
            Farm2.GetComponent<FarmSetting>().FarmCropSettingBlue();
        }
        else if (ML == MissionLocation.farm3)
        {
            Farm3.GetComponent<FarmSetting>().FarmCropSettingBlue();
        }
        else if (ML == MissionLocation.myGarden) {
            MyGarden.GetComponent<MyGardenSetting>().FlowerSettingBlue();
        }
    }

    void Heatstroke(MissionLocation ML) {
        if (ML == MissionLocation.farm2)
        {

        }
        else if (ML == MissionLocation.farm3)
        {

        }
        else if (ML == MissionLocation.mine)
        {

        }
    }

    void Fire(MissionLocation ML) {
        Debug.Log("Location: " + ML.ToString() + " Mission type is fire");
        if (ML == MissionLocation.forest)
        {
            Forest.GetComponent<FireRandomInit>().RandomFirePosition();
        }
        else if (ML == MissionLocation.mountain)
        {
            Forest.GetComponent<FireRandomInit>().RandomFirePosition();
        }
    }

    public void MissionComplete() {
        todayMissionCount -= 1;

        if (todayMissionCount == 0) { 
            //UI ������ �и鼭 �����
            //���� ������ ���ư���. ��� ���� UI ����
        }
    }
}