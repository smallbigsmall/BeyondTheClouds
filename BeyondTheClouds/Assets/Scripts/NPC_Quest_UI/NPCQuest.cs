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
                    DialogueIndex = 0; //���� ����. �ϻ� ��ȭ�� �ִ� index�� �������ִ� �κ�
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
                    DialogueIndex = Random.Range(0, dialogueList.Missions[missionIndex].Dialogue.Count);
                }  
                    //MyGarden Drought �̼��� index1�� day1 ���� �̼��̹Ƿ� ���ܵǵ��� �ϱ�
            }
            DialogueCanvas.SetActive(true);
            npcName = DialogueCanvas.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            LineText = DialogueCanvas.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
            //��ȭ
            lineTotalCount = dialogueList.Missions[missionIndex].Dialogue[DialogueIndex].Lines.Count;
            LineText.text = dialogueList.Missions[missionIndex].Dialogue[DialogueIndex].Lines[currentLineIndex].line;
            npcName.text = dialogueList.Name;
        }
        else {
            //�Ϲ� ��ȭ
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
        LineText.text = "�����մϴ�!";
    }

    public void RejectQuest() {
        ChoiceUI.SetActive(false);
        DialogueCanvas.SetActive(false);
        QuestPopup.SetActive(true);
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
