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
    private int QuestListIndex = -1, lineTotalCount = 0, currentLineIndex = 0;
    private bool canChangeLine = false, isQuestAccepted = false;
    private Button acceptButton, rejectButton;

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
                    QuestListIndex = dialogueList.Dialogues.Count - 1; //임의 설정. 일상 대화가 있는 index로 설정해주는 부분
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
        newQuest.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = dialogueList.Dialogues[QuestListIndex].Quest;
        GetComponentInParent<MissionSettingWithQuest>().QuestUI = newQuest;
        GetComponentInParent<MissionSettingWithQuest>().mapQuestMark = MapQuestMark;
    }

    public void MakeNPCQuest(string Mission) {
        isQuestNPC = true;
        QuestPopup.SetActive(true);
        MapQuestMark.SetActive(true);
        //AddQuestUI(); //For test. 나중에 이 부분 지우기.
    }

    public void StartConversation() {
        canChangeLine = true;
        currentLineIndex = 0;
        QuestPopup.SetActive(false);
        if (isQuestNPC)
        {
            if (QuestListIndex == -1) {
                for (int i = 0; i < dialogueList.Dialogues.Count; i++) {
                    if (dialogueList.Dialogues[i].Day == day) {
                        Debug.Log("current day for the dialogue is " + dialogueList.Dialogues[i].Day);
                        QuestListIndex = i;
                        Debug.Log("QuestListIndex is " + QuestListIndex);
                        break;
                    }
                }
                if(QuestListIndex == -1)
                    QuestListIndex = Random.Range(0, dialogueList.Dialogues.Count);
            }
            DialogueCanvas.SetActive(true);
            npcName = DialogueCanvas.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            LineText = DialogueCanvas.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
            //대화
            lineTotalCount = dialogueList.Dialogues[QuestListIndex].Lines.Count;
            LineText.text = dialogueList.Dialogues[QuestListIndex].Lines[currentLineIndex].line;
            npcName.text = dialogueList.Name;
        }
        else {
            //일반 대화
            DialogueCanvas.SetActive(true);
            QuestListIndex = dialogueList.Dialogues.Count - 1;
            npcName = DialogueCanvas.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            LineText = DialogueCanvas.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
            lineTotalCount = dialogueList.Dialogues[QuestListIndex].Lines.Count;
            LineText.text = dialogueList.Dialogues[QuestListIndex].Lines[currentLineIndex].line;
            npcName.text = dialogueList.Name;
        }
    }

    public void NextLine() {
        currentLineIndex++;
        if (currentLineIndex < lineTotalCount)
        {
            LineText.text = dialogueList.Dialogues[QuestListIndex].Lines[currentLineIndex].line;
        }
        else {
            if (isQuestNPC)
            {
                canChangeLine = false;
                ChoiceUI.SetActive(true);

                ChoiceUILineText = ChoiceUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
                ChoiceUILineText.text = dialogueList.Dialogues[QuestListIndex].Lines[lineTotalCount - 1].line;
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

    /*
     * 이대로 하면 NPC한테 다시 말걸었을때 대화가 처음부터 나옴...일상 대화 나오게 할까
     * NPC마다 변수 넣어주기 번거로움 이거 Start할 떄 미리 찾게 하기
     * 생각해보니깐 버튼한테 이벤트를 인스펙터에서 주면 안됨... 대화창 켜질때마다
     * 리스너 추가했다 뺐다 해야될듯?
     */
}
