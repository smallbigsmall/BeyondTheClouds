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

    //미션이름 대문자로 시작할 것
    public enum MissionType { None, Drought, Overwatering, Heatstroke, Fire, Cleaning, Random};
    public enum MissionLocation { None, myGarden, farm2, farm3, waterfall, forest, mountain, mine, Random };

    [Serializable]
    public class MissionInform { //미션 세부 내용
        
        public MissionType Mission_Type;
        public MissionLocation Location;

        public MissionInform() { }
        public MissionInform(MissionType t, MissionLocation l) {
            Mission_Type = t;
            Location = l;
        }        
    }

    [Serializable]
    public class MissionListOfDay { //하루치 미션 리스트
        public int day;
        public List<MissionInform> missionList = new List<MissionInform>();

        public MissionListOfDay() { }
        public MissionListOfDay(int d, List<MissionInform> ml) {
            day = d;
            missionList = ml;
        }
    }

    public int currentDay = 0; //for test
    public List<MissionListOfDay> missionListOfDay = new List<MissionListOfDay>(); //모든 day의 미션

    [SerializeField] GameObject MyGarden, Farm2, Farm3, Waterfall, Forest, Mountain, Mine;

    private int todayMissionCount = -1, randomStartIndex = -1;

    [SerializeField] GameObject QuestUIPrefab, QuestScrollView;


    //void Start()
    //{
    //    StartMissoinSetting(0);
    //}

    void makeNewDayMission() {
        int day = currentDay;
        List<MissionInform> newMissions = new List<MissionInform>();
        for(int i = 0; i < 3; i++)
            newMissions.Add(new MissionInform(MissionType.Random, MissionLocation.Random));

        if(Application.isPlaying)
        missionListOfDay.Add(new MissionListOfDay(day, newMissions));
        //7일 이후부터는 특정 날짜에 특정 미션을 추가하고 싶을 시 if문 이용하여 추가하기
        //상황에 따라 새로운 미션 추가되는 날짜만 인스펙터에서 지정하는 것도 고려
    }

    public void StartMissoinSetting(int index, int day) { //인스펙터에서 지정시 무조건 랜덤보다 수동으로 지정된 미션이 앞으로 오도록
        List<MissionInform> todayMission;
        currentDay = day;

        if (currentDay > 7 && index == 0)
        {
            makeNewDayMission();
        }

        todayMission = missionListOfDay[currentDay].missionList;
        todayMissionCount = todayMission.Count;

        for (int i = index; i < todayMissionCount; i++) {
            switch (todayMission[i].Mission_Type) {
                case MissionType.Drought:
                    Drought(todayMission[i].Location, todayMission[i].Mission_Type);
                    break;
                case MissionType.Overwatering:
                    Overwatering(todayMission[i].Location, todayMission[i].Mission_Type);
                    break;
                case MissionType.Heatstroke:
                    Heatstroke(todayMission[i].Location, todayMission[i].Mission_Type);
                    break;
                case MissionType.Fire:
                    Fire(todayMission[i].Location, todayMission[i].Mission_Type);
                    break;
                case MissionType.Cleaning:
                    Cleaning(todayMission[i].Location, todayMission[i].Mission_Type);
                    break;
                case MissionType.Random:
                    RandomMissionSetting(i);
                    break;
                default:    
                    break;
            }
        }
    }

    void RandomMissionSetting(int index) {

        if (randomStartIndex == -1) {
            randomStartIndex = index;
        }

        int tempLocationNum = 0, tempTypeNum = 0;
        bool checkAvailableLocation = false;
        MissionLocation tempLocation = MissionLocation.None;
        MissionType tempType = MissionType.None;

        while (!checkAvailableLocation) {
            checkAvailableLocation = true;
            tempLocationNum = UnityEngine.Random.Range(1, 8);
            for (int i = 0; i < index; i++)
            {
                if (tempLocationNum == (int)missionListOfDay[currentDay].missionList[i].Location)
                {
                    checkAvailableLocation = false;
                }
            }
        }

        tempLocation = (MissionLocation)Enum.ToObject(typeof(MissionLocation), tempLocationNum);

        //확장 버전의 경우 미션 추가되는 내용에 따라 if문 안에 day별로 나눠야할 수 도 있음
        if (tempLocation == MissionLocation.farm2 || tempLocation == MissionLocation.farm3)
        {
            tempTypeNum = UnityEngine.Random.Range(1, 4);
            tempType = (MissionType)Enum.ToObject(typeof(MissionType), tempTypeNum);
        } else if(tempLocation == MissionLocation.myGarden)
        {
            tempTypeNum = UnityEngine.Random.Range(1, 3);
            tempType = (MissionType)Enum.ToObject(typeof(MissionType), tempTypeNum);
        }
        else if (tempLocation == MissionLocation.waterfall)
        {
            tempType = MissionType.Drought;
        }
        else if (tempLocation == MissionLocation.forest || tempLocation == MissionLocation.mountain)
        {
            tempType = MissionType.Fire;
        }
        else if (tempLocation == MissionLocation.mine)
        {
            tempType = MissionType.Heatstroke;
        }

        missionListOfDay[currentDay].missionList[index].Location = tempLocation;
        missionListOfDay[currentDay].missionList[index].Mission_Type = tempType;

        if (index == missionListOfDay[currentDay].missionList.Count - 1) {
            StartMissoinSetting(randomStartIndex, currentDay);
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
            Mine.GetComponent<MineMission>().ChildHeatStrokeSetting();
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

    void Cleaning(MissionLocation ML, MissionType MT) {
        if (ML == MissionLocation.myGarden) {
            //Cleaning 미션 초기화되는 부분 찾아서 넣기
            RoomCleaner roomCleaner = FindAnyObjectByType<RoomCleaner>();
            roomCleaner.InitializeRoomCleanMission();

            CallChildQuestMethod(MyGarden, MissionType.Cleaning.ToString());
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

        if (NPCobj != null)
        {
            NPCobj.GetComponent<NPCQuest>().MakeNPCQuest(Mission, currentDay);
        }
        else {//봐서 플레이어용 퀘스트 스크립트 만들 수도 있음
            NPCobj = GameObject.FindWithTag("Player");
            NPCobj.GetComponent<NPCQuest>().MakeNPCQuest(Mission, currentDay);
        }
    }
}
