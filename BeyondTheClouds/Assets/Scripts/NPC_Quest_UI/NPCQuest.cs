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
    public GameObject DialogueCanvas, ChoiceUI;
    private TextMeshProUGUI LineText, npcName, ChoiceUILineText;
    public int day; //For test
    private int DialogueIndex = -1, lineTotalCount = 0, currentLineIndex = 0, missionIndex = -1;
    private bool canChangeLine = false, isQuestAccepted = false;
    private Button acceptButton, rejectButton;
    private string mission = "None";

    DialogueList dialogueList;
    [SerializeField] TextAsset jsonDialogue;

    [SerializeField] GameObject MapQuestMark;

    void Start()
    {
        QuestUIScrollView = GameObject.FindGameObjectWithTag("QuestUIScrollView");
 
        dialogueList = JsonUtility.FromJson<DialogueList>(jsonDialogue.text);
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
                    DialogueIndex = 0; //임의 설정. 일상 대화가 있는 index로 설정해주는 부분
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
        GetComponentInParent<MissionSettingWithQuest>().QuestUI = newQuest;
        GetComponentInParent<MissionSettingWithQuest>().mapQuestMark = MapQuestMark;
    }

    public void MakeNPCQuest(string Mission) {
        isQuestNPC = true;
        QuestPopup.SetActive(true);
        MapQuestMark.SetActive(true);
        mission = Mission;
    }

    public void StartConversation() {
        canChangeLine = true;
        currentLineIndex = 0;
        QuestPopup.SetActive(false);
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
                    DialogueIndex = Random.Range(0, dialogueList.Missions[missionIndex].Dialogue.Count);
                }  
                    //MyGarden Drought 미션의 index1은 day1 한정 미션이므로 제외되도록 하기
            }
            DialogueCanvas.SetActive(true);
            npcName = DialogueCanvas.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            LineText = DialogueCanvas.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
            //대화
            lineTotalCount = dialogueList.Missions[missionIndex].Dialogue[DialogueIndex].Lines.Count;
            LineText.text = dialogueList.Missions[missionIndex].Dialogue[DialogueIndex].Lines[currentLineIndex].line;
            npcName.text = dialogueList.Name;
        }
        else {
            //일반 대화
            DialogueCanvas.SetActive(true);
            missionIndex = 0;
            DialogueIndex = 0;
            npcName = DialogueCanvas.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            LineText = DialogueCanvas.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
            lineTotalCount = dialogueList.Missions[missionIndex].Dialogue[DialogueIndex].Lines.Count;
            LineText.text = dialogueList.Missions[missionIndex].Dialogue[DialogueIndex].Lines[currentLineIndex].line;
            npcName.text = dialogueList.Name;
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
        LineText.text = "감사합니다!";
    }

    public void RejectQuest() {
        ChoiceUI.SetActive(false);
        DialogueCanvas.SetActive(false);
        QuestPopup.SetActive(true);
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
