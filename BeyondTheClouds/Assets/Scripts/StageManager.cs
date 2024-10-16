using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    // Start is called before the first frame update
    private int currentStage = 0;
    private int totalStageCount = 6;
    private bool dayCleared = false;
    private PlayerData playerData;

    [SerializeField]
    private Transform stageListUI;

    [SerializeField]
    private GameObject stageButtonPref;

    [SerializeField]
    private Transform prologueBtn, endingBtn;

    private void Awake() {
        if (PlayerDataManager.Instance != null) {
            playerData = PlayerDataManager.Instance.LoadPlayerData();
        }
    }
    void Start()
    {
        for(int i = 1; i <= totalStageCount; i++) {
            InitializeStage(i);
        }

        if (playerData.stageNum < totalStageCount) endingBtn.GetComponent<Button>().interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeStage(int idx) {
        GameObject stageBtn = Instantiate(stageButtonPref);
        stageBtn.transform.SetParent(stageListUI, true);
        stageBtn.transform.SetSiblingIndex(idx);
        stageBtn.transform.localScale = new Vector3(1, 1, 1);
        stageBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text += (" " + idx);

        Stage initialStage = stageBtn.transform.GetComponent<Stage>();
        initialStage.SetStageNum(idx);

        if (playerData.stageNum > idx) {
            initialStage.SetDayCleared(true);
            initialStage.SetNightCleared(true);
        }
        else if (playerData.stageNum < idx) {
            initialStage.SetDayCleared(false);
            initialStage.SetNightCleared(false);
            stageBtn.transform.GetComponent<Button>().interactable = false;
        }
        else {
            if (playerData.dayCleared) {
                initialStage.SetDayCleared(true);
                initialStage.SetNightCleared(false);
                Debug.Log("Current stage: " + idx + " And you cleared daytime");
            }
            else {
                initialStage.SetDayCleared(false);
                initialStage.SetNightCleared(false);
                Debug.Log("Current stage: " + idx + " Start daytime mission");
            }
        }

    }

   
}
