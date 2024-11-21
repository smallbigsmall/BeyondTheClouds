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
    private string playerAccept = "����. ���� �ذ��غ���!";
    private string playerReject = "�ƴϾ�. ���� �� ����غ���.";
    private string NPCAccept = "�˾Ҿ�. ���� �����ٰ�!";
    private string NPCReject = "�̾���. ������ �� �غ���.";
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
                    DialogueIndex = 0; //���� ����. �ϻ� ��ȭ�� �ִ� index�� �������ִ� �κ�
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
            
            if (missionIndex == -1) { //Ȯ�� �� �̼��� �̼ǸŴ����ʿ��� �������� �����Ǳ⶧���� ���⼭ ���� �ȵ�������
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
                        //MyGarden Drought �̼��� index1�� day1 ���� �̼��̹Ƿ� ���ܵǵ��� �ϱ�
                        DialogueIndex = Random.Range(1, dialogueList.Missions[missionIndex].Dialogue.Count);
                    }
                    else {
                        DialogueIndex = Random.Range(0, dialogueList.Missions[missionIndex].Dialogue.Count);
                    }
                }  
            }
        }
        else {
            //�Ϲ� ��ȭ
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
            LineText.text = "�����մϴ�!";
        }
        else {
            LineText.text = "�� �� �� �����ž�!";
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
     * start�ϸ� �̸� �Ľ��� ��������.
     * NPC Ŭ���Ǹ� StartConversation() �����.
     * ���� ������������ Day �������� ��� ���.
     * ���� �� Ȯ�� ���������� �����ϰ� �̼��� �������Ƿ� ���õ� Mission �����ϰ� 
     * index �������� ������ ���� ��� ���
     * �÷��̾ ����Ʈ �޾Ƶ��̸� ����Ʈ UI�� �ؽ�Ʈ �������ְ� Add �ϱ�
     */

    /*
     * 1. �̼� �Ŵ����� �� ������ �̼� ������
     * 2. �̶� �� NPC�� ���µ� �̼�����/���� ���·� �ٲ�
     * 3. ����Ʈ ��ȭ ������ NPC�� QuestUI�� ����.
     * 4. ����Ʈ�� ������ Mission�� �������� json �ؽ�Ʈ ������. Index�� ��� ���� �ƴϸ�
     * �������� ����
     * �������� json�� ���� ������ٰű� ������ ���� ��ġ�� npc�� �Ҵ�� json�� ����
     * ������ �˾Ƽ� ���е� ����
     */
}
