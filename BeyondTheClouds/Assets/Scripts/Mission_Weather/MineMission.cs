using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineMission : MissionSettingWithQuest
{
    [SerializeField] WeatherMissionManager _weatherMissionManager;

    public void ChildHeatStrokeSetting()
    {
        transform.GetChild(0).gameObject.GetComponent<HeatStroke>().MakeNPCHeatStroke(_weatherMissionManager);
    }
}
