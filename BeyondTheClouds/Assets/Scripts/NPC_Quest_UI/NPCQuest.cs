using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPCQuest : MonoBehaviour
{
    [SerializeField] GameObject QuestUI;
    private GameObject QuestUIScrollView;
    private bool isQuestNPC = false;
    [SerializeField] GameObject QuestPopup;
    private GameObject newQuest;
    private GameObject DialogueCanvas, ChoiceUI;
    private TextMeshProUGUI LineText, npcName, ChoiceUILineText;
    private int day;
    private int DialogueIndex = -1, lineTotalCount = 0, currentLineIndex = 0, missionIndex = -1;
    private bool canChangeLine = false, isQuestAccepted = false, isPlayer = false;
    private Button acceptButton, rejectButton;
    private string mission = "None";
    private string playerAccept = "좋아. 내가 해결해보자!";
    private string playerReject = "아니야. 아직 더 고민해볼래.";
    private string NPCAccept = "알았어. 내가 도와줄게!";
    private string NPCReject = "미안해. 생각을 좀 해볼게.";
    private Image portraitImage;

    DialogueList_NPC dialogueList;
    [SerializeField] TextAsset jsonDialogue;
    private GameObject playerDialogueCanvas;

    public GameObject MapQuestMark;

    void Start()
    {
        QuestUIScrollView = GameObject.FindGameObjectWithTag("QuestUIScrollView");
 
        dialogueList = JsonUtility.FromJson<DialogueList_NPC>(jsonDialogue.text);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canChangeLine) {
            if (isQuestNPC)
            {
                if (!isQuestAccepted)
                {
                    NextLine();
                }
                else
                {
                    AddQuestUI();
                    isQuestNPC = false;
                    DialogueCanvas.SetActive(false);
                    canChangeLine = false;
                    acceptButton.onClick.RemoveListener(() => { AcceptQuest(); });
                    rejectButton.onClick.RemoveListener(() => { RejectQuest(); });
                    if (isPlayer) playerDialogueCanvas.SetActive(false);
                    DialogueIndex = 0; //임의 설정. 일상 대화가 있는 index로 설정해주는 부분
                    if (day == 1 && mission.Equals("Cleaning")) {
                        DialogueIndex = -1;
                        lineTotalCount = 0;
                        currentLineIndex = 0;
                        missionIndex = -1;
                        FindAnyObjectByType<WeatherMissionManager>().CallMyGardenQuestMethod();
                        RoomCleaner roomCleaner = FindAnyObjectByType<RoomCleaner>();
                        roomCleaner.InitializeRoomCleanMission();
                    }
                }
            }
            else {
                NextLine();
            }
        }
    }

    public void AddQuestUI() {
        newQuest = Instantiate(QuestUI);
        newQuest.transform.SetParent(QuestUIScrollView.transform);
        newQuest.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = dialogueList.Missions[missionIndex].Dialogue[DialogueIndex].Quest;
        if (!isPlayer)
        {
            GetComponentInParent<MissionSettingWithQuest>().QuestUI = newQuest;
            GetComponentInParent<MissionSettingWithQuest>().mapQuestMark = MapQuestMark;
        }
        else {
            GameObject myGarden = GameObject.FindWithTag("MyGarden");
            myGarden.GetComponent<MissionSettingWithQuest>().QuestUI = newQuest;
            myGarden.GetComponent<MissionSettingWithQuest>().mapQuestMark = MapQuestMark;
        }
    }

    public void MakeNPCQuest(string Mission, int day) {
        this.day = day;
        isQuestNPC = true;
        QuestPopup.SetActive(true);
        mission = Mission;
        isQuestAccepted = false;
        MapQuestMark.SetActive(true);
    }

    public void StartConversation() {
        canChangeLine = true;
        currentLineIndex = 0;
        QuestPopup.SetActive(false);
        if (isPlayer) playerDialogueCanvas.SetActive(false);

        if (isQuestNPC)
        {
            
            if (missionIndex == -1) { //확장 시 미션은 미션매니저쪽에서 랜덤으로 설정되기때문에 여기서 랜덤 안돌려도됨
                for (int i = 0; i < dialogueList.Missions.Count; i++) {
                    if (dialogueList.Missions[i].Mission.Equals(mission)) {
                        missionIndex = i;
                        break;
                    }
                }
            }
            if (DialogueIndex == -1) {
                for (int i = 0; i < dialogueList.Missions[missionIndex].Dialogue.Count; i++) {
                    if (dialogueList.Missions[missionIndex].Dialogue[i].Day == day) {
                        Debug.Log("current day for the dialogue is " + dialogueList.Missions[missionIndex].Dialogue[i].Day);
                        DialogueIndex = i;
                        Debug.Log("DialogueIndex is " + DialogueIndex);
                        break;
                    }
                }
                if (DialogueIndex == -1) {
                    if (dialogueList.Missions[missionIndex].Mission == "Drought" && isPlayer)
                    {
                        //MyGarden Drought 미션의 index1은 day1 한정 미션이므로 제외되도록 하기
                        DialogueIndex = Random.Range(1, dialogueList.Missions[missionIndex].Dialogue.Count);
                    }
                    else {
                        DialogueIndex = Random.Range(0, dialogueList.Missions[missionIndex].Dialogue.Count);
                    }
                }  
            }
        }
        else {
            //일반 대화
            if (isPlayer) return;
            missionIndex = 0;
            DialogueIndex = 0;
        }

        DialogueCanvas.SetActive(true);
        npcName = DialogueCanvas.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        LineText = DialogueCanvas.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        portraitImage = DialogueCanvas.transform.GetChild(0).GetChild(0).GetChild(2).gameObject.GetComponent<Image>();

        lineTotalCount = dialogueList.Missions[missionIndex].Dialogue[DialogueIndex].Lines.Count;
        LineText.text = dialogueList.Missions[missionIndex].Dialogue[DialogueIndex].Lines[currentLineIndex].line;
        npcName.text = dialogueList.Name;

        switch (gameObject.name)
        {
            case "DialogueCollider":
                if(gameObject.transform.parent.gameObject.name.Equals("MPlayer(Clone)")) 
                    portraitImage.sprite = Resources.Load<Sprite>("Portrait/Portrait_Alex");
                else if(gameObject.transform.parent.gameObject.name.Equals("FPlayer(Clone)"))
                    portraitImage.sprite = Resources.Load<Sprite>("Portrait/Portrait_Amelia");
                break;
            case "NPC_Adam":
                portraitImage.sprite = Resources.Load<Sprite>("Portrait/Portrait_Adam");
                break;
            case "NPC_Bob":
                portraitImage.sprite = Resources.Load<Sprite>("Portrait/Portrait_Bob");
                break;
        }
    }

    public void NextLine() {
        currentLineIndex++;
        if (currentLineIndex < lineTotalCount)
        {
            LineText.text = dialogueList.Missions[missionIndex].Dialogue[DialogueIndex].Lines[currentLineIndex].line;
        }
        else {
            if (isQuestNPC)
            {
                canChangeLine = false;
                ChoiceUI.SetActive(true);

                ChoiceUILineText = ChoiceUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
                ChoiceUILineText.text = dialogueList.Missions[missionIndex].Dialogue[DialogueIndex].Lines[lineTotalCount - 1].line;
                acceptButton = ChoiceUI.transform.GetChild(0).gameObject.GetComponent<Button>();
                rejectButton = ChoiceUI.transform.GetChild(1).gameObject.GetComponent<Button>();
                acceptButton.onClick.AddListener(() => { AcceptQuest(); });
                rejectButton.onClick.AddListener(() => { RejectQuest(); });
                if (isPlayer)
                {
                    ChoiceUI.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = playerAccept;
                    ChoiceUI.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = playerReject;
                }
                else {
                    ChoiceUI.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = NPCAccept;
                    ChoiceUI.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = NPCReject;
                }
            }
            else {
                DialogueCanvas.SetActive(false);
                canChangeLine = false;
            }
        }
    }

    public void AcceptQuest() {
        canChangeLine = true;
        isQuestAccepted = true;
        ChoiceUI.SetActive(false);
        if (gameObject.CompareTag("NPC"))
        {
            LineText.text = "감사합니다!";
        }
        else {
            LineText.text = "잘 할 수 있을거야!";
        }
    }

    public void RejectQuest() {
        ChoiceUI.SetActive(false);
        DialogueCanvas.SetActive(false);
        QuestPopup.SetActive(true);
        if (isPlayer) playerDialogueCanvas.SetActive(true);
    }

    public void SetDialogueUI(GameObject DialogueCanvas, GameObject ChoiceUI) {
        this.DialogueCanvas = DialogueCanvas;
        this.ChoiceUI = ChoiceUI;
    }

    public void SetIsPlayerTrue() {
        isPlayer = true;
    }

    public void setPlayerDialogueButtonCanvas(GameObject canvas) {
        playerDialogueCanvas = canvas;
    }

    /*
     * start하면 미리 파싱은 끝내놓기.
     * NPC 클릭되면 StartConversation() 실행됨.
     * 엔딩 보기전까지는 Day 기준으로 대사 출력.
     * 엔딩 후 확장 버전에서는 랜덤하게 미션이 정해지므로 선택된 Mission 참조하고 
     * index 랜덤으로 돌려서 나온 대사 출력
     * 플레이어가 퀘스트 받아들이면 퀘스트 UI의 텍스트 설정해주고 Add 하기
     */

    /*
     * 1. 미션 매니저가 각 구역에 미션 셋팅함
     * 2. 이때 각 NPC의 상태도 미션있음/없음 상태로 바꿈
     * 3. 퀘스트 대화 끝나면 NPC는 QuestUI를 만듦.
     * 4. 퀘스트의 내용은 Mission를 기준으로 json 텍스트 가져옴. Index의 경우 랜덤 아니면
     * 수동으로 설정
     * 영역별로 json을 따로 만들어줄거기 때문에 씬에 배치된 npc에 할당된 json에 따라
     * 영역은 알아서 구분될 예정
     */
}
