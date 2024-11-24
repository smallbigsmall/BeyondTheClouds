using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    int stageNum;
    int totalStageCount;
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

        totalStageCount = _stageManager.GetTotalStageCount();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnStageButtonClicked() {
        _stageManager.SetSelectedStage(stageNum);
        Debug.Log("Stage Button Clicked: " + stageNum);

        if(stageNum > totalStageCount + 1) { // plus game
            _stageManager.ActivatePopUp(false);
        }
        else if (stageNum <= totalStageCount + 1 && dayCleared) {
            _stageManager.ActivatePopUp(true);
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
