using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    // Start is called before the first frame update
    private int selectedStage = 0;
    private int totalStageCount = 6;
    private bool dayCleared = false;
    private PlayerData playerData;
    private Button yesBtn, noBtn;

    [SerializeField]
    private Transform stageListUI;

    [SerializeField]
    private GameObject stageButtonPref, popUpUI;

    [SerializeField]
    private Transform prologueBtn, endingBtn;

    private void Awake() {
        if (PlayerDataManager.Instance != null) {
            playerData = PlayerDataManager.Instance.LoadPlayerData();
        }
    }
    void Start()
    {
        yesBtn = popUpUI.transform.GetChild(1).GetComponent<Button>();
        noBtn = popUpUI.transform.GetChild(2).GetComponent<Button>();

        yesBtn.onClick.AddListener(OnYesButtonClicked);
        noBtn.onClick.AddListener(OnNoButtonClicked);

        for (int i = 1; i <= totalStageCount; i++) {
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
            stageBtn.transform.GetChild(1).GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        }
        else if (playerData.stageNum < idx) {
            initialStage.SetDayCleared(false);
            initialStage.SetNightCleared(false);
            stageBtn.transform.GetComponent<Button>().interactable = false;
        }
        else {
            stageBtn.transform.GetChild(1).GetComponent<Image>().color = new Color32(255, 255, 255, 0);
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

    public void OnPrologueButtonClicked() {
        selectedStage = 0;
        if (playerData.stageNum > 0 || (playerData.stageNum == 0 &&playerData.dayCleared)) {
            popUpUI.SetActive(true);
            popUpUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text 
                = "프롤로그 스토리를 본 적이 있습니다.\n다시 보시겠습니까?";
        }
    }

    public void OnEndingButtonClicked() {
        selectedStage = totalStageCount;
    }

    public void ActivatePopUp() { // In stage, day time clear
        popUpUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text
                = "Day time 게임을 이미 클리어했습니다.\n다시 Day time부터 플레이하시겠습니까?";
        popUpUI.SetActive(true);
    }

    private void OnYesButtonClicked() {
        if(selectedStage == 0) {  //prologue
            Debug.Log("Load prologue story cut scene");
        }
        else if(selectedStage < totalStageCount){ //stage
            // Daytime play again?
            Debug.Log($"Play {selectedStage} day game");
        }
        GameManager.Instance.SetCurrentPlayerData(selectedStage, false);
        GameManager.Instance.LoadMainMap();
        popUpUI.SetActive(false);
    }

    private void OnNoButtonClicked() {
        if (selectedStage == 0) {  //prologue
            //SceneManager.LoadScene("MainMap");
            Debug.Log("Play first night game");
        }
        else if (selectedStage < totalStageCount) { //stage
            // Daytime play again?
            Debug.Log($"Play {selectedStage} night game");           
        }

        GameManager.Instance.SetCurrentPlayerData(selectedStage, true);
        GameManager.Instance.LoadMainMap();
        popUpUI.SetActive(false);
    }

    public void SetSelectedStage(int stage) {
        if(stage>0 && stage< totalStageCount) {
            selectedStage = stage;
        }
    }
}
