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


    void Start()
    {
        StartMissoinSetting();
    }

    void StartMissoinSetting() {
        List<MissionInform> todayMission = missionListOfDay[currentDay].missionList;
        int size = todayMission.Count;

        for (int i = 0; i < size; i++) {
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
        Debug.Log("Location: " + ML.ToString() + " Mission type is drought");
    }

    void Overwatering(MissionLocation ML) {
        Debug.Log("Location: " + ML.ToString() + " Mission type is overwatering");
    }

    void Heatstroke(MissionLocation ML) {
        Debug.Log("Location: " + ML.ToString() + " Mission type is heatstroke");
    }

    void Fire(MissionLocation ML) {
        Debug.Log("Location: " + ML.ToString() + " Mission type is fire");
    }
}
