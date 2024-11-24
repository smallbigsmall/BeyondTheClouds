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
    private int totalStageCount = 5;
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
            playerData = PlayerDataManager.Instance.GetPlayerData();
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
        else if (playerData.stageNum > totalStageCount + 1) InitializePlusStage();
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

    private void InitializePlusStage() {
        GameObject stageBtn = Instantiate(stageButtonPref);
        stageBtn.transform.SetParent(stageListUI, true);
        stageBtn.transform.SetSiblingIndex(totalStageCount+2);
        stageBtn.transform.localScale = new Vector3(1, 1, 1);
        stageBtn.transform.GetChild(1).GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        stageBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Plus\nStage";
        Stage initialStage = stageBtn.GetComponent<Stage>();
        initialStage.SetStageNum(playerData.stageNum);
    }

    public void OnPrologueButtonClicked() {
        selectedStage = 0;
        if (playerData.stageNum > 0 || (playerData.stageNum == 0 &&playerData.dayCleared)) {
            popUpUI.SetActive(true);
            popUpUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text 
                = "프롤로그 스토리를 본 적이 있습니다.\n다시 보시겠습니까?";
        }else if(playerData.stageNum == 0 && !playerData.dayCleared) {
            SceneManager.LoadScene("Prologue");
        }
    }

    public void OnEndingButtonClicked() {
        selectedStage = totalStageCount + 1;

        if (playerData.stageNum > selectedStage || (playerData.stageNum == selectedStage && playerData.dayCleared)) {
            popUpUI.SetActive(true);
            popUpUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text
                = "Day time 게임을 이미 클리어했습니다.\n다시 Day time부터 플레이하시겠습니까?";
        }else if(playerData.stageNum == selectedStage && !playerData.dayCleared) {
            SceneManager.LoadScene("MainMap");
        }
    }

    public void ActivatePopUp(bool isDefaultGame) { // In stage, day time clear
        if (isDefaultGame) {
            popUpUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text
                = "Day time 게임을 이미 클리어했습니다.\n다시 Day time부터 플레이하시겠습니까?";
        }
        else {
            popUpUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text
                = "Day time과 Night time 중\n어느 게임을 플레이하시겠습니까?";
            // yes -> day , no -> night
            popUpUI.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Day";
            popUpUI.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Night";
        }
        
        popUpUI.SetActive(true);
    }

    private void OnYesButtonClicked() {
        if(selectedStage == 0) {  //prologue
            SceneManager.LoadScene("Prologue");
            return;
        }
        else if(selectedStage <= totalStageCount){ //stage
            // Daytime play again?
            Debug.Log($"Play {selectedStage} day game");
        }
        else if(selectedStage == totalStageCount + 1) { //ending
            Debug.Log("Play Ending Daytime");
        }else if(selectedStage == totalStageCount + 2) {
            Debug.Log("Play plus stage");
        }
        GameManager.Instance.SetCurrentPlayerData(selectedStage, false); 
        GameManager.Instance.LoadMainMap();
        popUpUI.SetActive(false);
    }

    private void OnNoButtonClicked() {
        GameManager.Instance.SetCurrentPlayerData(selectedStage, true);

        if (selectedStage == 0) {  //prologue
            //SceneManager.LoadScene("MainMap");
            Debug.Log("Play first night game");
        }
        else if (selectedStage <= totalStageCount) { //stage
            // Daytime play again?
            Debug.Log($"Play {selectedStage} night game");           
        }
        else if(selectedStage == totalStageCount + 1) { //ending
            SceneManager.LoadScene("Ending");
            return;
        }
       
        GameManager.Instance.LoadMainMap();
        popUpUI.SetActive(false);
    }

    public void SetSelectedStage(int stage) {
        if(stage>0) {
            selectedStage = stage;
        }
    }

    public int GetTotalStageCount() {
        return totalStageCount;
    }

    public void GoToStartScene() {
        SceneManager.LoadScene("StartScene");
    }
}
