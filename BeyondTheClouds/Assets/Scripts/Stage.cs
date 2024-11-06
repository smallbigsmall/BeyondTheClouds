using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    int stageNum;
    bool dayCleared;
    bool nightCleared;

    Button myBtn;
    StageManager _stageManager;
   
    // Start is called before the first frame update
    void Start()
    {
        _stageManager = FindAnyObjectByType<StageManager>();
        myBtn = GetComponent<Button>();
        myBtn.onClick.AddListener(OnStageButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnStageButtonClicked() {
        _stageManager.SetSelectedStage(stageNum);
        Debug.Log("Stage Button Clicked: " + stageNum);

        //daytime clear? -> show pop-up
        if (dayCleared) {
            _stageManager.ActivatePopUp();
        }
        else {
            Debug.Log($"Play {stageNum} day game");
            GameManager.Instance.SetCurrentPlayerData(stageNum, false);
            GameManager.Instance.LoadMainMap();
        }
        //not cleared -> just play day time game
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
