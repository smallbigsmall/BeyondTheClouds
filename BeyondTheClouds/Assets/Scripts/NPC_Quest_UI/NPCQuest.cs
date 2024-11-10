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
                    QuestListIndex = dialogueList.Dialogues.Count - 1; //���� ����. �ϻ� ��ȭ�� �ִ� index�� �������ִ� �κ�
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
        //AddQuestUI(); //For test. ���߿� �� �κ� �����.
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
            //��ȭ
            lineTotalCount = dialogueList.Dialogues[QuestListIndex].Lines.Count;
            LineText.text = dialogueList.Dialogues[QuestListIndex].Lines[currentLineIndex].line;
            npcName.text = dialogueList.Name;
        }
        else {
            //�Ϲ� ��ȭ
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

    /*
     * �̴�� �ϸ� NPC���� �ٽ� ���ɾ����� ��ȭ�� ó������ ����...�ϻ� ��ȭ ������ �ұ�
     * NPC���� ���� �־��ֱ� ���ŷο� �̰� Start�� �� �̸� ã�� �ϱ�
     * �����غ��ϱ� ��ư���� �̺�Ʈ�� �ν����Ϳ��� �ָ� �ȵ�... ��ȭâ ����������
     * ������ �߰��ߴ� ���� �ؾߵɵ�?
     */
}
