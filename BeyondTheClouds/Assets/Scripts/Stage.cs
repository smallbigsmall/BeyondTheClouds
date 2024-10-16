using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    int stageNum;
    bool dayCleared;
    bool nightCleared;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStageNum(int num) {
        if(num >= 1)
        stageNum = num;
    }

    public int GetStageNum() {
        return stageNum;
    }

    public void SetDayCleared(bool cleared) {
        dayCleared = cleared;
    }

    public bool GetDayCleared() {
        return dayCleared;
    }

    public void SetNightCleared(bool cleared) {
        nightCleared = cleared;
    }

    public bool GetNightCleared() {
        return nightCleared;
    }
}
