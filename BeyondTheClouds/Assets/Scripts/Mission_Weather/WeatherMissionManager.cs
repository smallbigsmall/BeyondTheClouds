using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeatherMissionManager : MonoBehaviour
{
    //이벤트1(1일차)
    //  이벤트세부1
    //  이벤트 종류
    //  이벤트 장소
    //
    //  이벤트세부2
    //  이벤트 종류
    //  이벤트 장소

    public enum MissionType { None, drought, overwatering, heatstroke, fire, Random};
    public enum MissionLocation { None, myGarden, farm2, farm3, waterfall, forest, mountain, mine, Random };

    [Serializable]
    public class MissionInform { //미션 세부 내용
        
        public MissionType Mission_Type;
        public MissionLocation Location;
    }

    [Serializable]
    public class MissionListOfDay { //하루치 미션 리스트
        public List<MissionInform> missionList = new List<MissionInform>();
    }

    public int currentDay = 0; //for test
    public List<MissionListOfDay> missionListOfDay = new List<MissionListOfDay>(); //모든 day의 미션

    [SerializeField] GameObject MyGarden, Farm2, Farm3, Waterfall, Forest, Mountain, Mine;
    [SerializeField] GameObject MissionUIPrefab; //UI 상태 변경하는 스크립트 붙이기, UI 만들고 미션 스크립트에 넘겨주기

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
            //UI 옆으로 밀면서 숨기기
            //이제 집으로 돌아가자. 라고 쓰인 UI 띄우기
        }
    }
}
