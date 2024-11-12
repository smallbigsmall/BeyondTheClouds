using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaterfallMission : MissionSettingWithQuest
{
    [SerializeField] WeatherMissionManager _weatherMissionManager;
    [SerializeField] GameObject waterfallSoilObj;

    public void missionSetting() {
        waterfallSoilObj.SetActive(true);
    }

    public void MissionComplete() {
        _weatherMissionManager.MissionComplete();
        CompleteQuestUI();
    }

    //public void ChildHeatStrokeSetting() {
    //    transform.GetChild(1).GetComponent<HeatStroke>().MakeNPCHeatStroke(_weatherMissionManager);
    //}
}
