using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

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


    //여기서 NPC한테 퀘스트 마크 띄우게끔 시킬건데 My Garden의 경우
    //NPC가 없으므로 스킵. 대신 플레이어한테 퀘스트 마크가 떠야함

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

    private int todayMissionCount = -1;

    [SerializeField] GameObject QuestUIPrefab, QuestScrollView;

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
                    Drought(todayMission[i].Location, todayMission[i].Mission_Type);
                    break;
                case MissionType.overwatering:
                    Overwatering(todayMission[i].Location, todayMission[i].Mission_Type);
                    break;
                case MissionType.heatstroke:
                    Heatstroke(todayMission[i].Location, todayMission[i].Mission_Type);
                    break;
                case MissionType.fire:
                    Fire(todayMission[i].Location, todayMission[i].Mission_Type);
                    break;
                default:    
                    break;
            }
        }
    }

    void Drought(MissionLocation ML, MissionType MT) {
        if (ML == MissionLocation.farm2)
        {
            Farm2.GetComponent<FarmSetting>().FarmCropSettingYellow();
            CallChildQuestMethod(Farm2, MT.ToString());
        }
        else if (ML == MissionLocation.farm3)
        {
            Farm3.GetComponent<FarmSetting>().FarmCropSettingYellow();
            CallChildQuestMethod(Farm3, MT.ToString());
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
            CallChildQuestMethod(Waterfall, MT.ToString());
        }
    }

    void Overwatering(MissionLocation ML, MissionType MT) {
        Debug.Log("Location: " + ML.ToString() + " Mission type is overwatering");
        if (ML == MissionLocation.farm2)
        {
            Farm2.GetComponent<FarmSetting>().FarmCropSettingBlue();
            CallChildQuestMethod(Farm2, MT.ToString());
        }
        else if (ML == MissionLocation.farm3)
        {
            Farm3.GetComponent<FarmSetting>().FarmCropSettingBlue();
            CallChildQuestMethod(Farm3, MT.ToString());
        }
        else if (ML == MissionLocation.myGarden) {
            MyGarden.GetComponent<MyGardenSetting>().FlowerSettingBlue();
        }
    }

    void Heatstroke(MissionLocation ML, MissionType MT) {
        if (ML == MissionLocation.farm2)
        {
            Farm2.GetComponent<FarmSetting>().ChildHeatStrokeSetting();
            CallChildQuestMethod(Farm2, MT.ToString());
        }
        else if (ML == MissionLocation.farm3)
        {
            Farm3.GetComponent<FarmSetting>().ChildHeatStrokeSetting();
            CallChildQuestMethod(Farm3, MT.ToString());
        }
        else if (ML == MissionLocation.mine)
        {
            CallChildQuestMethod(Mine, MT.ToString());
        }
    }

    void Fire(MissionLocation ML, MissionType MT) {
        Debug.Log("Location: " + ML.ToString() + " Mission type is fire");
        if (ML == MissionLocation.forest)
        {
            Forest.GetComponent<FireRandomInit>().RandomFirePosition();
            CallChildQuestMethod(Forest, MT.ToString());
        }
        else if (ML == MissionLocation.mountain)
        {
            Mountain.GetComponent<FireRandomInit>().RandomFirePosition();
            CallChildQuestMethod(Mountain, MT.ToString());
        }
    }

    public void MissionComplete() {
        todayMissionCount -= 1;

        if (todayMissionCount == 0) {
            Debug.Log("이제 집으로 돌아가자");
            GameObject newQuest = Instantiate(QuestUIPrefab);
            newQuest.transform.SetParent(QuestScrollView.transform);
            newQuest.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "이제 집으로\n돌아가자!";
        }
    }

    public void CallChildQuestMethod(GameObject area, string Mission) {
        int temp = area.transform.childCount;
        GameObject NPCobj = null;

        for (int i = 0; i < temp; i++) {
            if (area.transform.GetChild(i).gameObject.CompareTag("NPC")) {
                NPCobj = area.transform.GetChild(i).gameObject;
                break;
            }
        }

        if (NPCobj != null) {
            NPCobj.GetComponent<NPCQuest>().MakeNPCQuest(Mission);
        }
    }
}
