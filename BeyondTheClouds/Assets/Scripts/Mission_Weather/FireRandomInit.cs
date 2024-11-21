using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRandomInit : MissionSettingWithQuest
{
    private int fireCount;
    [SerializeField] WeatherMissionManager _weatherMissionManager;

    public void RandomFirePosition() {
        fireCount = transform.GetChild(0).childCount;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void FireComplete() {
        fireCount -= 1;
        if (fireCount == 0) {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            _weatherMissionManager.MissionComplete();
            CompleteQuestUI();
        }
    }

    //public void ChildHeatStrokeSetting()
    //{
    //    transform.GetChild(1).GetComponent<HeatStroke>().MakeNPCHeatStroke(_weatherMissionManager);
    //}
}
